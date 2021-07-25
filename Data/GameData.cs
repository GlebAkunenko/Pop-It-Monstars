using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public float Difficulty { get; set; }
    public string LocationName { get; set; }
    public bool Reviewed { get; set; }

    public Location[] Locations { get; set; }

    private Location currentLevelCache;

    public Location CurrentLocation
    {
        get
        {
            if (currentLevelCache != null) {
                if (currentLevelCache.Name == LocationName)
                    return currentLevelCache;
            }
            foreach (Location location in Locations) {
                if (location.Name == LocationName) {
                    currentLevelCache = location;
                    return location;
                }
            }
            return null;
        }
    }

}