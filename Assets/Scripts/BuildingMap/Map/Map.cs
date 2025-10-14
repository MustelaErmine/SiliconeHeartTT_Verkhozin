using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using System.Linq;

public class Map : MonoBehaviour, IInitializable, ILoadable
{
    [Inject]
    private SaveProvider provider;
    private MapData _mapData;

    [Inject]
    private Settings settings;

    [Inject]
    private BuildingIndex _buildingIndex;

    private Dictionary<Guid, GameObject> _buildingObjects;

    private Dictionary<Guid, AssetLoader> _references;

    public void Initialize()
    {
        _references = new Dictionary<Guid, AssetLoader>();
        _buildingObjects = new Dictionary<Guid, GameObject>();

        Load();
        PasteFromData();
    }
    public void PasteFromData()
    {
        Guid[] guids = _buildingObjects.Keys.ToArray();
        foreach (var building in guids)
        {
            DeleteBuilding(building);
        }
        foreach (var building in _mapData.Buildings)
        {
            _references.Add(building.GUID, new AssetLoader());
            var config = _buildingIndex.ByGuid(building.old);

            StartCoroutine(_references[building.GUID].GetAsset<Sprite>(config.sprite, (sprite) =>
            {
                var buildingEditData = new BuildingEditData();
                buildingEditData.display = new BuildingDisplayData();
                buildingEditData.display.sprite = sprite;
                buildingEditData.display.sizeX = config.sizeX;
                buildingEditData.display.sizeY = config.sizeY;
                buildingEditData.data = new BuildingData();
                buildingEditData.data.positionX = building.positionX;
                buildingEditData.data.positionY = building.positionY;
                buildingEditData.data.GUID = building.GUID;

                PlaceBuilding(buildingEditData);
            }));
        }
    }

    public void PlaceBuilding(BuildingEditData buildingEditData)
    {
        if (IsCompatible(buildingEditData))
        {
            SetBuilding(buildingEditData);
        }
    }
    public void CreateBuilding(Guid guid)
    {
        Guid newguid = Guid.NewGuid();
        _references.Add(newguid, new AssetLoader());
        var config = _buildingIndex.ByGuid(guid);

        StartCoroutine(_references[newguid].GetAsset<Sprite>(config.sprite, (sprite) =>
        {
            _buildingObjects[newguid] = Instantiate(settings.buildingPrefab, transform);
            var display = _buildingObjects[newguid].GetComponent<BuildingDisplay>();
            var interaction = display.GetComponent<BuildingInteraction>();

            var buildingEditData = new BuildingEditData();
            buildingEditData.display = new BuildingDisplayData();
            buildingEditData.display.sprite = sprite;
            buildingEditData.display.sizeX = config.sizeX;
            buildingEditData.display.sizeY = config.sizeY;

            buildingEditData.data = new BuildingData();
            buildingEditData.data.positionX = int.MaxValue;
            buildingEditData.data.positionY = int.MaxValue;
            buildingEditData.data.GUID = newguid;
            buildingEditData.data.old = guid;

            display.Display(buildingEditData.display);

            interaction.BuildingEditData = buildingEditData;
            interaction.map = this;

            interaction.StartDrag(Input.mousePosition);

            _mapData.Buildings.Add(buildingEditData.data);
        }));
    }
    bool IsCompatible(BuildingEditData buildingEditData) 
    {
        BuildingData data = buildingEditData.data;
        int x0 = data.positionX, x1 = data.positionX + buildingEditData.display.sizeX;
        int y0 = data.positionY, y1 = data.positionY + buildingEditData.display.sizeY;

        HashSet<(int, int)> set = new HashSet<(int, int)>();

        for (int i = x0; i < x1; i++)
        {
            for (int j = y0; j < y1; j++)
            {
                set.Add((i, j));
            }
        }

        foreach(var building in _mapData.Buildings)
        {
            if (building.GUID == buildingEditData.data.GUID)
                continue;

            var indexedBuilding = _buildingIndex.ByGuid(building.old);
            int bx0 = building.positionX, bx1 = building.positionX + indexedBuilding.sizeX;
            int by0 = building.positionY, by1 = building.positionY + indexedBuilding.sizeY;

            for (int i = bx0; i < bx1; i++)
            {
                for (int j = by0; j < by1; j++)
                {
                    if (set.Contains((i, j)))
                        return false;
                }
            }
        }
        return true;
    }
    void SetBuilding(BuildingEditData buildingEditData)
    {
        GameObject building = null;
        BuildingInteraction buildingInteraction = null;

        if (_buildingObjects.ContainsKey(buildingEditData.data.GUID))
        {
            building = _buildingObjects[buildingEditData.data.GUID];
        }
        else
        {
            building = Instantiate(settings.buildingPrefab);
            _buildingObjects[buildingEditData.data.GUID] = building;
        }

        building.GetComponent<BuildingDisplay>().Display(buildingEditData.display);
        buildingInteraction = building.GetComponent<BuildingInteraction>();

        buildingInteraction.BuildingEditData = buildingEditData;
        buildingInteraction.map = this;
        buildingInteraction.SetPosition();

        _mapData.SetByGuid(buildingEditData.data.GUID, buildingEditData.data);
    }
    public void TryDelete(Guid guid)
    {
        DeleteBuilding(guid);
        DeleteStateEnd();
    }

    void DeleteBuilding(Guid guid)
    {
        Destroy(_buildingObjects[guid]);
        _references[guid].Release();

        _buildingObjects.Remove(guid);
        _references.Remove(guid);

        _mapData.DeleteByGuid(guid);
    }
    public void DeleteState()
    {
        foreach (var building in _buildingObjects)
        {
            building.Value.GetComponent<BuildingInteraction>().State = new BuildingInteraction.Delete();
        }
    }
    public void DeleteStateEnd()
    {
        foreach (var building in _buildingObjects)
        {
            building.Value.GetComponent<BuildingInteraction>().State = new BuildingInteraction.None();
        }
    }

    public void Load()
    {
        _mapData = provider.Current.mapData;
        PasteFromData();
    }

    public class Settings
    {
        public GameObject buildingPrefab;
    }
}
