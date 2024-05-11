using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTips_Sprite : MonoBehaviour
{
    [SerializeField] private string tipsToShow;
    [SerializeField] private float timeToWait = 0.5f;

    public void InitTipsText(string value)
    {
        tipsToShow = value;
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

    private void ShowTips()
    {
        TooltipsManager.OnMouseHover_Sprite(tipsToShow, Input.mousePosition);
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);

        ShowTips();
    }
}
