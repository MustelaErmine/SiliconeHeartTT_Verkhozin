using System;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;

public class SaveProvider
{
    [Serializable]
    public class Data
    {
        public MapData mapData = new MapData();
    }

    public Data Current;

    readonly string SavePath = Application.persistentDataPath + "/save.yaml";

    public SaveProvider()
    {
        Current = new Data();
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Current = new Data();
        }
        var deserializer = new DeserializerBuilder().Build();
        using (FileStream stream = File.OpenRead(SavePath))
        {
            using (TextReader textReader = new StreamReader(stream))
            {
                Current = deserializer.Deserialize<Data>(textReader);
            }
        }
    }
    
    public void Save()
    {
        var serializer = new SerializerBuilder().Build();
        using (FileStream stream = File.OpenWrite(SavePath))
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                serializer.Serialize(writer, Current);
            }
        }
    }
}