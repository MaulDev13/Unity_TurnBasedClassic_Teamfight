using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public delegate void UnitEvent();
    public UnitEvent iUpdate;
    public UnitEvent iMove;
    public UnitEvent iAction;
    public UnitEvent iTakeDamage;
    public UnitEvent iDead;

    public delegate void UnitEvent2(Skill _skill);
    public UnitEvent2 iAction2;

    [SerializeField] private BattleUnitAnimation myAnim;

    [SerializeField] public Unit myUnit;

    [SerializeField] public List<BattleUnit> myEnemies = new List<BattleUnit>();
    [SerializeField] public List<BattleUnit> myAllies = new List<BattleUnit>();

    [HideInInspector] public bool isAlive;
    [SerializeField] public bool isPlayerUnit = true;

    [SerializeField] public bool isAuto = true;

    [SerializeField] private float timeToEndTurn = 0.2f;
    private float autoDelayTime = 0.2f;

    private bool isAct;
    public bool IsAct => isAct;

    public virtual void Init(Unit _unit, bool _isPlayerUnit, string setName)
    {
        isPlayerUnit = _isPlayerUnit;

        myUnit = _unit.Clone();
        myUnit.Init(_unit);

        myAnim.Init(myUnit.art);

        if (isPlayerUnit)
        {
            //myUnit.unitName = $"Player".ToString();
            myUnit.unitName = $"Player" + setName;
        }
        else
        {
            myUnit.unitName = $"AI" + setName;
        }

        ZeroShield();

        myUnit.currentMovement = 0f;

        isAlive = true;
        isAct = false;

        autoDelayTime = LocalManager_Arena.instance.enemyDelayAct;

        // Debugging
        //myUnit.healthPoint = Random.Range(25f, 75f);

        iUpdate?.Invoke();
    }

    public virtual void Turn()
    {
        CooldownSkill();

        iUpdate?.Invoke();

        isAct = true;

        if (!isPlayerUnit || isAuto)
        {
            StartCoroutine(MovementAI());
        }

        iUpdate?.Invoke();
    }

    // AI have a selected skill
    IEnumerator MovementAI(int _index)
    {
        yield return new WaitForSeconds(autoDelayTime);

        SelectTarget(myUnit.skillSet[_index]);

        yield return new WaitForSeconds(autoDelayTime);
    }

    // AI use random selection for search of skill
    IEnumerator MovementAI()
    {
        yield return new WaitForSeconds(autoDelayTime);

        bool checkAct = false;
        int index = 0;
        while (!checkAct)
        {
            index = Random.Range(0, myUnit.skillSet.Count);

            if (myUnit.skillSet[index].CheckCD())
            {
                checkAct = true;

                SelectTarget(myUnit.skillSet[index]);
            }

            yield return new WaitForSeconds(autoDelayTime);
        };
    }

    public virtual void SelectTarget(Skill s)
    {
        if (!isAct)
            return;

        if(!isPlayerUnit || isAuto)
        {
            if(!ArenaSelection.instance.AutoSelect(this, s))
            {
                Debug.Log("Invalid skill, can't find minimal target");
                StartCoroutine(MovementAI());
            }

        }
        else
        {
            ArenaSelection.instance.Init(this, s);
        }
    }

    public virtual void Action(Skill s, List<BattleUnit> targets)
    {
        if (!isAct)
            return;

        isAct = false;

        iAction?.Invoke();
        iAction2?.Invoke(s);

        s.Use(this, targets);
        LocalManager_ArenaUI.instance.LastMove($"#{LocalManager_Arena.instance.CurrentTurn} - {s.CreateLastMove(this, targets)}");

        Invoke("EndTurn", timeToEndTurn);
    }

    public virtual void EndTurn()
    {
        iUpdate?.Invoke();

        if (LocalManager_Arena.instance.State == LocalManager_Arena.BattleState.OnTurn)
            LocalManager_Arena.instance.EndTurn();
    }

    public float Attack(BattleUnit target, float _value, AttackType _attackType)
    {
        var value = target.TakeDamage(_value, _attackType);

        return value;
    }

    public float Healing(BattleUnit target, float _value)
    {
        var value = target.Heal(_value);

        return value;
    }

    public float Heal(float _value)
    {
        _value = Mathf.Round(_value);

        if (ChangeHealth(_value) <= 0)
        {
            Dead();
        }

        iUpdate?.Invoke();

        return _value;
    }

    public float TakeDamage(float _value, AttackType _attackType)
    {
        if (_attackType == AttackType.Break && myUnit.shieldPoint > 0)
            _value *= 2f;
        else if(_attackType == AttackType.Simple && myUnit.shieldPoint <= 0)
            _value *= 2f;

        if (myUnit.shieldPoint > 0 && _attackType != AttackType.Penetration)
        {
            if(myUnit.shieldPoint >= _value)
            {
                myUnit.shieldPoint -= _value;
                _value = 0;
            } else
            {
                _value -= myUnit.shieldPoint;
                myUnit.shieldPoint = 0;
            }
        }

        _value = Mathf.Round(_value);

        if (_value > 0)
            iTakeDamage?.Invoke();

        if (ChangeHealth(-_value) <= 0)
        {
            Dead();
        }

        iUpdate?.Invoke();

        return _value;
    }

    public virtual bool Dead()
    {
        isAlive = false;

        iDead?.Invoke();

        Debug.Log($"{myUnit.unitName} dead!");

        //LocalManager_Arena.instance.EndBattle(!isPlayerUnit);

        iUpdate?.Invoke();

        return true;
    }

    public void Move(float value)
    {
        myUnit.currentMovement = myUnit.currentMovement + value > 0 ? myUnit.currentMovement + value : 0;

        iMove?.Invoke();
    }

    public float AddMove()
    {
        var value = myUnit.movement >= 0 ? myUnit.movement : 0;
        return value;
    }


    public float GetMove()
    {
        return myUnit.currentMovement;
    }

    private float ChangeHealth(float value)
    {
        myUnit.healthPoint = Mathf.Round(Mathf.Clamp(myUnit.healthPoint + value, 0f, myUnit.maxHealtPoint));
        return myUnit.healthPoint;
    }

    public void CooldownSkill()
    {
        foreach(Skill s in myUnit.skillSet)
        {
            s.ReduceCD(1);
        }
    }

    public void ZeroShield()
    {
        myUnit.shieldPoint = 0f;

        iUpdate?.Invoke();
    }

    public void AddShield(float _value)
    {
        myUnit.shieldPoint += Mathf.Round(_value);

        iUpdate?.Invoke();
    }

    public string GetName()
    {
        return myUnit.unitName;
    }
}
