using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Journey", menuName = "Journey/New Journey")]
[Serializable]
public class Journey : ScriptableObject
{
    public string journeyName;

    public List<Road> roads = new List<Road>();

    public List<Road> GetRoad()
    {
        if (roads.Count < 1)
            return null;

        List<Road> tmpRoads = new List<Road>();

        for (int i = 0; i < roads.Count; i++)
        {
            var tmpRoad = roads[i].Clone();
            tmpRoad.roadKey = tmpRoads.Count;
            tmpRoad.SetActive();
            tmpRoads.Add(tmpRoad);
        }

        return tmpRoads;
    }

    public bool AddRoad(Road _road)
    {
        var tmpRoad = _road.Clone();
        tmpRoad.roadKey = roads.Count;
        tmpRoad.SetActive();
        roads.Add(tmpRoad);

        return true;
    }
}
