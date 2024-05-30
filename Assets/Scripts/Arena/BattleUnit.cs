using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public delegate void UnitEvent();
    public UnitEvent iUpdate;
    public UnitEvent iMove;
    public UnitEvent iDead;

    public delegate void UnitEvent2(Skill _skill);
    public UnitEvent2 iAction2;
    public UnitEvent2 iTakeDamage2;
    public UnitEvent2 iHeal2;
    public UnitEvent2 iShield2;

    public delegate void UnitEvent3(Mod _mod);
    public UnitEvent3 iAction3;
    public UnitEvent3 iTakeDamage3;
    public UnitEvent3 iHeal3;
    public UnitEvent3 iShield3;

    [SerializeField] private BattleUnitAnimation myAnim;

    [SerializeField] public Unit myUnit;

    [SerializeField] public List<BattleUnit> myEnemies = new List<BattleUnit>();
    [SerializeField] public List<BattleUnit> myAllies = new List<BattleUnit>();

    [SerializeField] public List<Mod> modActives = new List<Mod>();

    [HideInInspector] public bool isAlive;
    [SerializeField] public bool isPlayerUnit = true;

    [SerializeField] public bool isAuto = true;

    [SerializeField] private float timeToEndTurn = 0.2f;
    private float autoDelayTime = 0.2f;

    [SerializeField] public List<Mod> mods = new List<Mod>();

    public List<BattleUnit> currentTarget = new List<BattleUnit>();

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

        for (int i = 0; i < myUnit.passiveSkills.Count; i++)
        {
            if (myUnit.passiveSkills[i].CheckCD())
            {
                myUnit.passiveSkills[i].Use(this);
            }
        }

        for(int i = 0; i < modActives.Count; i++)
        {
            if(modActives[i].modType == ModType.FirstTurn)
            {
                modActives[i].Active();
            }
        }

        iUpdate?.Invoke();
    }

    public virtual void Turn()
    {
        CooldownSkill();

        currentTarget.Clear();

        for (int i = 0; i < modActives.Count; i++)
        {
            if (modActives[i].modType == ModType.StartTurn)
            {
                modActives[i].Active();
            }
        }

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

        iAction2?.Invoke(s);

        currentTarget.AddRange(targets);

        s.Use(this, targets);
        LocalManager_ArenaUI.instance.LastMove($"#{LocalManager_Arena.instance.CurrentTurn} - {s.CreateLastMove(this, targets)}");

        Invoke("EndTurn", timeToEndTurn);
    }

    public virtual void Action(Skill s)
    {
        if (!isAct)
            return;

        isAct = false;

        if(s.target == SkillTarget.AllAllies)
            currentTarget.AddRange(myAllies);
        if(s.target == SkillTarget.AllEnemies)
            currentTarget.AddRange(myEnemies);
        if(s.target == SkillTarget.All)
        {
            currentTarget.AddRange(myAllies);
            currentTarget.AddRange(myEnemies);
        }


        iAction2?.Invoke(s);

        s.Use(this);
        LocalManager_ArenaUI.instance.LastMove($"#{LocalManager_Arena.instance.CurrentTurn} - {s.CreateLastMove(this)}");

        Invoke("EndTurn", timeToEndTurn);
    }

    public virtual void EndTurn()
    {
        iUpdate?.Invoke();

        if (LocalManager_Arena.instance.State == LocalManager_Arena.BattleState.OnTurn)
            LocalManager_Arena.instance.EndTurn();
    }

    public float Attack(BattleUnit target, float _value, AttackType _attackType, Skill _skill)
    {
        var value = target.TakeDamage(_value, _attackType, _skill);

        return value;
    }

    public float Healing(BattleUnit target, float _value, Skill _skill)
    {
        var value = target.Heal(_value, _skill);

        return value;
    }

    public float Heal(float _value, Skill _skill)
    {
        _value = Mathf.Round(_value);

        if (ChangeHealth(_value) <= 0)
        {
            Dead();
        }

        iUpdate?.Invoke();

        return _value;
    }

    public float TakeDamage(float _value, AttackType _attackType, Skill _skill)
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
            iTakeDamage2?.Invoke(_skill);

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

    public void AddShield(float _value, Skill _skill)
    {
        myUnit.shieldPoint += Mathf.Round(_value);

        iShield2?.Invoke(_skill);

        iUpdate?.Invoke();
    }

    public string GetName()
    {
        return myUnit.unitName;
    }
}
