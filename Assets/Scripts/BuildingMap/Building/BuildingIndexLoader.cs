using System;
using System.Collections;
using UnityEngine;
using YamlDotNet.Serialization;

public class BuildingIndexLoader
{
    public static IEnumerator GetIndex(Action<BuildingIndex> callback = null)
    {
        AssetLoader assetLoader = new AssetLoader();
        return assetLoader.GetAsset<TextAsset>("buildings_index", (asset) =>
        {
            var index = GetBuildingIndex(asset);

            assetLoader.Release();
            callback(index);
        });
    }
    public static BuildingIndex GetBuildingIndex (TextAsset asset)
    {
        var deserializer = new Deserializer();
        var index = deserializer.Deserialize<BuildingIndex>(asset.text);
        return index;
    }
}