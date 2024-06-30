using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalManager_Arena : MonoBehaviour
{
    #region Singleton
    public static LocalManager_Arena instance;

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

    [Header("Sync")]
    public float MovementThreshold = 100;

    public List<Transform> playerHolders = new List<Transform>();
    public List<Transform> enemyHolders = new List<Transform>();

    public GameObject unitPrefabs;

    public float enemyDelayAct = 0.5f;

    public bool isHideActBtnEnemy = true;

    [Header("Inspector")]
    [SerializeField] public List<BattleUnit> teamA = new List<BattleUnit>();
    [SerializeField] public List<BattleUnit> teamB = new List<BattleUnit>();

    [HideInInspector] public BattleUnit unitTurn;

    private bool isCheckTurn;

    public enum BattleState
    {
        Preperation, CheckTurn, OnTurn, Win, Defeat
    }
    [SerializeField] private BattleState state;
    public BattleState State => state;

    private int currentTurn;
    public int CurrentTurn => currentTurn;

    private void Start()
    {
        isCheckTurn = false;
        BattleStateManager(BattleState.Preperation);
    }

    private void FixedUpdate()
    {
        if (isCheckTurn)
        {
            CheckTurn();
        }
    }

    private void BattleStateManager(BattleState value)
    {
        state = value;

        if (state != BattleState.Preperation)
            CheckEnd();

        currentTurn++;

        LocalManager_ArenaUI.instance.TurnInfo();

        switch (state)
        {
            case BattleState.Preperation:
                Preperation();
                break;
            case BattleState.CheckTurn:
                isCheckTurn = true;
                break;
            case BattleState.OnTurn:
                isCheckTurn = false;

                if(unitTurn.isPlayerUnit || !isHideActBtnEnemy)
                    LocalManager_ArenaUI.instance.SetActionBtn();

                unitTurn.Turn();
                unitTurn.Move(-MovementThreshold);
                break;
            case BattleState.Win:
                // Win the battle
                Debug.Log("You Win!");

                isCheckTurn = false;

                //ArenaGameManager.instance.BattleResult(true);
                break;
            case BattleState.Defeat:
                // Lose the battle
                Debug.Log("You Lose!");

                isCheckTurn = false;

                //ArenaGameManager.instance.BattleResult(false);
                break;
        }
    }

    private void Preperation()
    {
        currentTurn = 0;

        int i = 0;
        foreach (Unit u in ArenaGameManager.instance.playerUnits)
        {
            var newUnit = Instantiate(unitPrefabs, playerHolders[i]) as GameObject;
            var battleScript = newUnit.GetComponent<BattleUnit>();
            battleScript.isPlayerUnit = true;
            battleScript.Init(u, true, i.ToString());

            teamA.Add(battleScript);

            var holderScript = playerHolders[i].GetComponent<UnitHolder>();
            holderScript.Init(battleScript, true);

            i++;
        }

        i = 0;
        foreach (Unit u in ArenaGameManager.instance.enemyUnits)
        {
            var newUnit = Instantiate(unitPrefabs, enemyHolders[i]) as GameObject;
            var battleScript = newUnit.GetComponent<BattleUnit>();
            battleScript.isPlayerUnit = false;
            battleScript.Init(u, false, i.ToString());

            teamB.Add(battleScript);

            var holderScript = enemyHolders[i].GetComponent<UnitHolder>();
            holderScript.Init(battleScript, true);

            i++;
        }

        
        // Check who is enemy
        foreach(BattleUnit u in teamA)
        {
            u.myAllies = teamA;
            u.myEnemies = teamB;
        }

        foreach(BattleUnit u in teamB)
        {
            u.myAllies = teamB;
            u.myEnemies = teamA;
        }

        LocalManager_ArenaUI.instance.SetUnitInfoPanel();

        BattleStateManager(BattleState.CheckTurn);
    }

    private void CheckEnd()
    {
        bool checkLiving = false;
        foreach(BattleUnit u in teamA)
        {
            if (u.isAlive)
            {
                checkLiving = true;
                break;
            }
        }

        if(!checkLiving)
        {
            state = BattleState.Defeat;
        }

        checkLiving = false;
        foreach (BattleUnit u in teamB)
        {
            if (u.isAlive)
            {
                checkLiving = true;
                break;
            }
        }

        if (!checkLiving)
            state = BattleState.Win;
    }

    private void CheckTurn()
    {
        BattleUnit tmpFrontUnit = null;

        foreach (BattleUnit u in teamA)
        {
            if (u.GetMove() > MovementThreshold && u.isAlive)
            {
                if (tmpFrontUnit == null)
                {
                    tmpFrontUnit = u;
                }
                else if (u.GetMove() > tmpFrontUnit.GetMove())
                {
                    tmpFrontUnit = u;
                }
            }
        }

        foreach (BattleUnit u in teamB)
        {
            if (u.GetMove() > MovementThreshold && u.isAlive)
            {
                if (tmpFrontUnit == null)
                {
                    tmpFrontUnit = u;
                }
                else if (u.GetMove() > tmpFrontUnit.GetMove())
                {
                    tmpFrontUnit = u;
                }
            }
        }

        if (tmpFrontUnit == null)
        {
            AddMovement();
        }
        else
        {
            unitTurn = tmpFrontUnit;
            BattleStateManager(BattleState.OnTurn);
        }
    }

    public void EndTurn()
    {
        LocalManager_ArenaUI.instance.ClearActionButton();

        BattleStateManager(BattleState.CheckTurn);
    }

    public void EndBattle(bool _isPlayerWin)
    {
        if(_isPlayerWin)
            BattleStateManager(BattleState.Win);
        else
            BattleStateManager(BattleState.Defeat);
    }

    public void Forfeit()
    {
        BattleStateManager(BattleState.Defeat);
    }

    private void AddMovement()
    {
        foreach (BattleUnit u in teamA)
        {
            u.Move(u.AddMove() * Time.fixedDeltaTime);
        }

        foreach (BattleUnit u in teamB)
        {
            u.Move(u.AddMove() * Time.fixedDeltaTime);
        }
    }
}