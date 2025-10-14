using System;

[Serializable]
public class BuidlingConfigData
{
    public int sizeX, sizeY;
    public string sprite;
    public string guid;

    public Guid Guid { get => new Guid(guid); }
}