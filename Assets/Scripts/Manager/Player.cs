using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    public List<Unit> teammate = new List<Unit>();
    public List<Road> activeRoads = new List<Road>();
    public List<Journey> avaliableJourneys = new List<Journey>();


    public void AddAvaliableJourney(Journey _journey)
    {
        avaliableJourneys.Add(_journey.Clone());
    }

    public void RemoveAvaliableJourney(Journey _journey)
    {
        foreach (Journey j in avaliableJourneys)
        {
            if (j.journeyName == _journey.journeyName)
            {
                avaliableJourneys.Remove(j);
            }
        }
    }

    public void SetActiveRoads(Journey journey)
    {
        activeRoads.Clear();
        activeRoads = journey.GetRoad();
    }

    public void RemoveRoad(Road _road)
    {
        foreach(Road r in activeRoads)
        {
            if(r.roadKey == _road.roadKey)
            {
                //r.isActive = false;
                activeRoads.Remove(r);
            }
        }
    }
}
