using System;
using System.Collections.Generic;

[Serializable]
public class MapData
{
    public List<BuildingData> Buildings = new List<BuildingData>();

    public void SetByGuid(Guid guid, BuildingData data)
    {
        foreach(BuildingData building in Buildings)
        {
            if (building.GUID == guid)
            {
                building.positionX = data.positionX;
                building.positionY = data.positionY;
                return;
            }
        }
        throw new ArgumentException();
    }
    public void DeleteByGuid(Guid guid)
    {
        Buildings.RemoveAll(building => building.GUID == guid);
    }
}