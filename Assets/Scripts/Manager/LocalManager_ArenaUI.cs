using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalManager_ArenaUI : MonoBehaviour
{
    #region Singleton
    public static LocalManager_ArenaUI instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    [SerializeField] protected GameObject actionBtnPrefabs;
    [SerializeField] private GameObject unitInfoPanelPrefabs;
    [SerializeField] protected GameObject lastMovePrefabs;
    [SerializeField] private GameObject movePointPrefabs;

    [SerializeField] protected Transform actionBtnPanelParent;
    [SerializeField] private Transform teamAUnitInfoPanelParent;
    [SerializeField] private Transform teamBUnitInfoPanelParent;
    [SerializeField] private Transform movePointParent;
    [SerializeField] protected Transform lastMoveParent;

    [SerializeField] protected GameObject endPanel;
    [SerializeField] protected TextMeshProUGUI endTxt;

    [SerializeField] protected TextMeshProUGUI turnInfoTxt;

    protected List<ArenaActionBtn> btnActionList = new List<ArenaActionBtn>();

    public virtual void SetUnitInfoPanel()
    {
        foreach (BattleUnit u in LocalManager_Arena.instance.teamA)
        {
            var newPanel = Instantiate(unitInfoPanelPrefabs, teamAUnitInfoPanelParent) as GameObject;
            var panelScript = newPanel.GetComponent<ArenaUnitInfoPanel>();
            panelScript.Init(u);

            var newPoint = Instantiate(movePointPrefabs, movePointParent) as GameObject;
            var pointScript = newPoint.GetComponent<ArenaMovePoint>();
            pointScript.Init(u, movePointParent.gameObject);
        }

        foreach (BattleUnit u in LocalManager_Arena.instance.teamB)
        {
            var newPanel = Instantiate(unitInfoPanelPrefabs, teamBUnitInfoPanelParent) as GameObject;
            var panelScript = newPanel.GetComponent<ArenaUnitInfoPanel>();
            panelScript.Init(u);

            var newPoint = Instantiate(movePointPrefabs, movePointParent) as GameObject;
            var pointScript = newPoint.GetComponent<ArenaMovePoint>();
            pointScript.Init(u, movePointParent.gameObject);
        }
    }

    public virtual void SetActionBtn()
    {
        // Create new button if button is less than skill set capacity
        if(LocalManager_Arena.instance.unitTurn.myUnit.skillSet.Count > btnActionList.Count)
        {
            for(int i = btnActionList.Count; i < LocalManager_Arena.instance.unitTurn.myUnit.skillSet.Count; i++)
            {
                var newBtn = Instantiate(actionBtnPrefabs, actionBtnPanelParent) as GameObject;
                var btnScript = newBtn.GetComponent<ArenaActionBtn>();
                btnActionList.Add(btnScript);
            }
        }

        ClearActionButton();

        // Set active the button
        for(int i = 0; i < LocalManager_Arena.instance.unitTurn.myUnit.skillSet.Count; i++)
        {
            btnActionList[i].gameObject.SetActive(true);
            btnActionList[i].Init(LocalManager_Arena.instance.unitTurn, LocalManager_Arena.instance.unitTurn.myUnit.skillSet[i]);
        }
    }

    public virtual void ClearActionButton()
    {
        // Set inactive all button
        for (int i = 0; i < btnActionList.Count; i++)
        {
            btnActionList[i].gameObject.SetActive(false);
        }
    }

    public virtual void TurnInfo()
    {
        switch (LocalManager_Arena.instance.State)
        {
            case LocalManager_Arena.BattleState.Preperation:
                turnInfoTxt.text = "Preperation";
                break;
            case LocalManager_Arena.BattleState.CheckTurn:
                turnInfoTxt.text = "Check Turn";
                break;
            case LocalManager_Arena.BattleState.OnTurn:
                turnInfoTxt.text = $"Turn #{LocalManager_Arena.instance.CurrentTurn} - {LocalManager_Arena.instance.unitTurn.GetName()}";
                break;
            case LocalManager_Arena.BattleState.Win:
                turnInfoTxt.text = "Player Win";
                EndBattle(true);
                break;
            case LocalManager_Arena.BattleState.Defeat:
                turnInfoTxt.text = "Player Defeat";
                EndBattle(false);
                break;
        }
    }

    public virtual void EndBattle(bool _isWin)
    {
        Debug.Log("end battle");

        if(_isWin)
        {
            endTxt.text = "You Win!";
        } else
        {
            endTxt.text = "Defeat!";
        }

        endPanel.SetActive(true);
    }

    public virtual void EndButton()
    {

        if (LocalManager_Arena.instance.State == LocalManager_Arena.BattleState.Win)
        {
            ArenaGameManager.instance.BattleResult(true);
        }
        else
        {
            ArenaGameManager.instance.BattleResult(false);
        }
    }

    public virtual void LastMove(string _value)
    {
        if (lastMoveParent == null || lastMovePrefabs == null)
        {
            //Debug.LogWarning("You can't display Last Move from Arena UI because you either not have last move parent or last move prefabs");
            return;
        }
            

        var newBtn = Instantiate(lastMovePrefabs, lastMoveParent) as GameObject;
        var btnScript = newBtn.GetComponent<ArenaLastMove>();
        btnScript.Init(_value);
    }
}
