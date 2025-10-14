using UnityEngine;
using Zenject;

public class MapBuildingInstaller : MonoInstaller
{
    [SerializeField] private TextAsset buildingIndex;
    [SerializeField] private GameObject iconPrefab, buildingPrefab;
    public override void InstallBindings()
    {
        Container.Bind<BuildingIndex>().FromInstance(BuildingIndexLoader.GetBuildingIndex(buildingIndex)).AsSingle().NonLazy();
        Container.Bind<PlaceNewBuilding.Settings>().FromInstance(new PlaceNewBuilding.Settings
        {
            iconPrefab = iconPrefab
        });
        Container.Bind<Map.Settings>().FromInstance(new Map.Settings { buildingPrefab = buildingPrefab}).AsSingle();
        Container.BindInterfacesAndSelfTo<Map>().FromComponentsInHierarchy(map => true).AsSingle();
        Container.BindInterfacesAndSelfTo<PlaceNewBuilding>().FromComponentsInHierarchy(_ => true).AsSingle();

        Container.Bind<SaveProvider>().FromNew().AsSingle().NonLazy();
    }
}