using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Index : MonoBehaviour
{
    #region Singleton
    public static Index instance;

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

    public List<Unit> unitIndex = new List<Unit>();

    public List<Journey> journeyIndex = new List<Journey>();

    public List<Road> roadIndex = new List<Road>();

}
