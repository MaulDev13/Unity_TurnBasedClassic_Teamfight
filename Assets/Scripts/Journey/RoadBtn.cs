using UnityEngine;
using TMPro;

public class RoadBtn : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private TextMeshProUGUI descTxt;

    private Road myRoad;

    public void Init(Road _road)
    {
        myRoad = _road;

        titleTxt.text = myRoad.roadName;
        descTxt.text = myRoad.SetDesc();
    }

    public void OnRoadBtn()
    {
        myRoad.Use();
    }
}
