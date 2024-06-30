using System;
using UnityEngine;

public enum RoadType
{
    Null,
    Battle,
    Campfire
}

[Serializable]
public class Road : ScriptableObject
{
    public string roadName;
    [HideInInspector] public int roadKey = 0;
    [HideInInspector] public bool isActive = true;

    public virtual void Use() 
    {
        Player.instance.RemoveRoad(this);
    }

    public virtual void SetActive()
    {
        isActive = true;
    }

    public virtual string SetDesc()
    {
        return "";
    }
}