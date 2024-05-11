using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public delegate void UnitEvent();
    public UnitEvent iUpdate;
    public UnitEvent iAction;
    public UnitEvent iTakeDamage;
    public UnitEvent iDead;

    public delegate void UnitEvent2(Skill _skill);
    public UnitEvent2 iAction2;

    [SerializeField] private BattleUnitAnimation myAnim;

    [SerializeField] public Unit myUnit;
    [HideInInspector] public BattleUnit myEnemy;

    [HideInInspector] public bool isAlive;
    [SerializeField] public bool isPlayerUnit = true;

    [SerializeField] private float timeToEndTurn = 0.2f;

    private bool isAct;
    public bool IsAct => isAct;

    public virtual void Init(Unit _unit, bool _isPlayerUnit)
    {
        isPlayerUnit = _isPlayerUnit;

        myUnit = _unit.Clone();
        myUnit.Init(_unit);

        myAnim.Init(myUnit.art);

        if (isPlayerUnit)
        {
            myUnit.unitName = $"Player".ToString(); ;
        }
        else
        {
            myUnit.unitName = $"AI".ToString();
        }

        ZeroShield();

        isAlive = true;
        isAct = false;

        iUpdate?.Invoke();
    }

    public virtual void Turn()
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        CooldownSkill();

        iUpdate?.Invoke();

        isAct = true;

        if (!isPlayerUnit)
        {
            StartCoroutine(MovementAI());
            //StartCoroutine(MovementAI2());
        }

        iUpdate?.Invoke();
    }

    // AI have a selected skill
    IEnumerator MovementAI(int _index)
    {
        yield return new WaitForSeconds(2f);

        Action(myUnit.skillSet[_index]);

        yield return new WaitForSeconds(0.2f);
    }

    // AI use random selection for search of skill
    IEnumerator MovementAI()
    {
        yield return new WaitForSeconds(2f);

        bool checkAct = false;
        int index = 0;
        while (!checkAct)
        {
            index = Random.Range(0, myUnit.skillSet.Count);

            if (myUnit.skillSet[index].CheckCD())
            {
                checkAct = true;

                Action(myUnit.skillSet[index]);
            }

            yield return new WaitForSeconds(0.2f);
        };
    }

    // AI search for best skill avaliable with reward base method
    IEnumerator MovementAI2()
    {
        yield return new WaitForSeconds(2f);

        float currentReward = 0f;
        float nextReward = 0f;
        int index = -1;

        for(int i = 0; i < myUnit.skillSet.Count; i++)
        {
            if (myUnit.skillSet[i].CheckCD())
            {
                nextReward = myUnit.skillSet[i].GetValue();
            }
            else
            {
                nextReward = 0;
            }

            if (currentReward < nextReward || currentReward == -1)
            {
                currentReward = nextReward;
                index = i;
            }
        }

        Action(myUnit.skillSet[index]);

        yield return new WaitForSeconds(0.2f);
    }

    public virtual void Action(Skill s)
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        if (!isAct)
            return;

        isAct = false;

        //iAction?.Invoke();
        iAction2?.Invoke(s);

        s.Use(this, myEnemy);
        LocalManager_ArenaUI.instance.LastMove($"#{LocalManager_Arena.instance.CurrentTurn} - {s.CreateLastMove(this, myEnemy)}");

        ArenaSelection.instance.Clear();

        Invoke("EndTurn", timeToEndTurn);
    }

    public virtual void EndTurn()
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        iUpdate?.Invoke();
        
        LocalManager_Arena.instance.EndTurn();
    }

    public float Heal(float _value)
    {
        if(ChangeHealth(_value) <= 0)
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

        if(_value > 0)
            iTakeDamage?.Invoke();

        if (ChangeHealth(-_value) <= 0)
        {
            Dead();
        }

        iUpdate?.Invoke();

        return _value;
    }

    public virtual void Dead()
    {
        if (LocalManager_Arena.instance.isEnd)
            return;

        isAlive = false;

        iDead?.Invoke();

        Debug.Log($"{myUnit.unitName} dead!");

        LocalManager_Arena.instance.EndBattle(!isPlayerUnit);

        iUpdate?.Invoke();
    }

    private float ChangeHealth(float value)
    {
        myUnit.healthPoint = Mathf.Clamp(myUnit.healthPoint + value, 0f, myUnit.maxHealtPoint);
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
    }

    public void AddShield(float _value)
    {
        myUnit.shieldPoint += _value;
    }

    public string GetName()
    {
        return myUnit.unitName;
    }
}
