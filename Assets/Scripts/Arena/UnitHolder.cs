using System.Collections;
using UnityEngine;

public class UnitHolder : MonoBehaviour
{
    [SerializeField] private Transform selectedFlag;

    [HideInInspector] public BattleUnit myBattleUnit;

    [HideInInspector] public bool onPlayerSide = true;

    [TextArea(8,4)]
    [SerializeField] private string tipsToShow;
    [SerializeField] private float timeToWait = 0.5f;

    bool isSelected;

    public void Init(BattleUnit battleUnit, bool isPlayerSide)
    {
        myBattleUnit = battleUnit;
        onPlayerSide = isPlayerSide;

        //tipsToShow = $"{myBattleUnit.myUnit.unitName}\n{myBattleUnit.myUnit.healthPoint} / {myBattleUnit.myUnit.maxHealtPoint}";
        tipsToShow = $"{myBattleUnit.myUnit.unitName}";
    }

    private void OnMouseEnter()
    {
        StopAllCoroutines();

        StartCoroutine(StartTimer());
    }

    private void OnMouseExit()
    {
        StopAllCoroutines();

        TooltipsManager.OnMouseLoseFocus_Sprite();
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"{myBattleUnit.myUnit.unitName} is selected");

        isSelected = ArenaSelection.instance.Select(myBattleUnit, isSelected);

        if(isSelected)
        {
            ArenaSelection.instance.iAction += SelectedFlag;

            selectedFlag.gameObject.SetActive(true);
        } else
        {
            ArenaSelection.instance.iAction -= SelectedFlag;

            selectedFlag.gameObject.SetActive(false);
        }
    }

    private void ShowTips()
    {
        TooltipsManager.OnMouseHover_Sprite(tipsToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowTips();
    }

    private void SelectedFlag()
    {
        ArenaSelection.instance.iAction -= SelectedFlag;

        selectedFlag.gameObject.SetActive(false);
        isSelected = false;
    }
}
