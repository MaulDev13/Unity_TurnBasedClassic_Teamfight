using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager_Journey : MonoBehaviour
{
    [SerializeField] private Transform roadParent;
    [SerializeField] private GameObject roadPrefabs;

    [SerializeField] private int maxRoadWindow = 5;

    private void Start()
    {
        Init();
    }

    void Init() 
    {
        if (Player.instance.activeRoads.Count < 1)
            return;

        // Set road avaliable
        for(int i = 0; i<maxRoadWindow; i++)
        {
            if (Player.instance.activeRoads.Count < i)
                break;

            var newRoad = Instantiate(roadPrefabs, roadParent) as GameObject;
            var newRoadScript = newRoad.GetComponent<RoadBtn>();
            newRoadScript.Init(Player.instance.activeRoads[i]);
        }
    }
}
