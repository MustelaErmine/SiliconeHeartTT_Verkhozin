using System;
using System.Collections.Generic;

[Serializable]
public class BuildingIndex
{
    public List<BuidlingConfigData> buildings;

    public BuidlingConfigData ByGuid(Guid guid)
    {
        foreach (var building in buildings)
        {
            if (new Guid(building.guid) == guid)
            {
                return building;
            }
        }
        throw new ArgumentException("guid");
    }
}
