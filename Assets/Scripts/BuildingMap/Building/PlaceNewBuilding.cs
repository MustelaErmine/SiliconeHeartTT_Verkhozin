using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlaceNewBuilding : MonoBehaviour, IInitializable
{
    [Inject]
    Map map;

    [Inject]
    BuildingIndex index;

    [Inject]
    Settings settings;

    [SerializeField] private CanvasGroup _canvasGroup;

    public void Initialize() 
    {
        PlaceIcons();
    }
    void PlaceIcons()
    {
        foreach (var building in index.buildings)
        {
            var icon = Instantiate(settings.iconPrefab, transform).GetComponent<NewBuildingIcon>();

            AssetLoader assetLoader = new AssetLoader();

            StartCoroutine(assetLoader.GetAsset<Sprite>(building.sprite, (sprite) =>
            {
                icon.Controller = this;
                icon.Data = new BuildingIconData
                {
                    guid = new Guid(building.guid),
                    sprite = sprite
                };
                icon.Initialize();
            }));
        }
    }
    public void HidePanel()
    {
        _canvasGroup.alpha = 1f;
    }

    public void Place(Guid guid)
    {
        map.CreateBuilding(guid);
    }
    public void DeleteState()
    {
        map.DeleteState();
    }

    public class Settings
    {
        public GameObject iconPrefab;
    }
}
