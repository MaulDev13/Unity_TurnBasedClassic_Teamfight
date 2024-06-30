using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTarget
{
    Allies,
    Enemies,
    Oneself,
    AllAllies,
    AllEnemies,
    All,
    Null
}

public enum AttackType
{
    Normal,
    Penetration, // Can't be block by shield
    Break, // Double damage when target have shield
    Simple // Double damage when target not have a shield
}

public enum BaseValue
{
    CurrentHealth,
    BaseHealth,
    LostHealth,
    Attack,
    Shield,
    Null
}

public enum Tags
{
    Null
}

[Serializable]
public class Skill : ScriptableObject
{
    [SerializeField] public AnimatorOverrideController _animator; // On act

    [SerializeField] public GameObject actOnSelft_Effect;
    [SerializeField] public GameObject actOnTarget_Effect;
    [SerializeField] public GameObject hit_Effect;

    [SerializeField] public string skillName;

    public int cd;
    [HideInInspector] public int currentCd = 0;
    public bool isStartWithZeroCD = true;

    [TextArea(4, 8)]
    [SerializeField] public string skillDesc;

    public SkillTarget target;
    public bool isLivingTarget = true;

    public int maxTarget = 1;
    public int minTarget = 1;

    [SerializeField] public bool isPassive = false;

    public List<Tags> skillTags = new List<Tags>();

    public virtual void Use(BattleUnit user) { }

    public virtual void Use(BattleUnit user, List<BattleUnit> targets)
    {
        currentCd = cd;
    }

    public virtual bool CheckCD()
    {
        if(currentCd <= 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public virtual void ReduceCD(int value)
    {
        currentCd = Mathf.Clamp(currentCd - value, 0, cd);
    }

    public virtual float GetValue(BattleUnit user, BaseValue _baseValue, float _additionalValue, float _baseDamageMultiplier)
    {
        switch (_baseValue)
        {
            case BaseValue.CurrentHealth:
                return _additionalValue + (_baseDamageMultiplier * user.myUnit.healthPoint);
            case BaseValue.BaseHealth:
                return _additionalValue + (_baseDamageMultiplier * user.myUnit.attackPoint);
            case BaseValue.LostHealth:
                return _additionalValue + (_baseDamageMultiplier * user.myUnit.LostHealth());
            case BaseValue.Shield:
                return _additionalValue + (_baseDamageMultiplier * user.myUnit.shieldPoint);
            case BaseValue.Attack:
                return _additionalValue + (_baseDamageMultiplier * user.myUnit.attackPoint);
            case BaseValue.Null:
                return _additionalValue;
            default:
                return 0f;
        }
    }

    public virtual string CreateLastMove(BattleUnit user, List<BattleUnit> targets)
    {
        Debug.Log($"{user.GetName()} use {skillName}");
        return $"{user.GetName()} use {skillName}";
    }

    public virtual string CreateLastMove(BattleUnit user)
    {
        Debug.Log($"{user.GetName()} use {skillName}");
        return $"{user.GetName()} use {skillName}";
    }

    public virtual void CreateDesc()
    {
        skillDesc = $"{skillName.ToUpper()} is a skill";
    }

    public virtual string GetTarget()
    {
        switch(target)
        {
            case SkillTarget.Allies:
                return "Allies";
            case SkillTarget.Enemies:
                return "Enemies";
            case SkillTarget.Oneself:
                return "Oneself";
            case SkillTarget.Null:
                return "Null";
            case SkillTarget.AllAllies:
                return "All allies";
            case SkillTarget.AllEnemies:
                return "All enemies";
            case SkillTarget.All:
                return "Everyone on arena";
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                return "default";
        }
    }

    public virtual string GetTarget(SkillTarget _target)
    {
        switch (_target)
        {
            case SkillTarget.Allies:
                return "Allies";
            case SkillTarget.Enemies:
                return "Enemies";
            case SkillTarget.Oneself:
                return "Oneself";
            case SkillTarget.Null:
                return "Null";
            case SkillTarget.AllAllies:
                return "All allies";
            case SkillTarget.AllEnemies:
                return "All enemies";
            case SkillTarget.All:
                return "Everyone on arena";
            default:
                Debug.LogWarning($"Skill {skillName} not have a target");
                return "default";
        }
    }

    public virtual string GetAttackType(AttackType _attackType)
    {
        switch (_attackType)
        {
            case AttackType.Normal:
                return "normal";
            case AttackType.Penetration:
                return "penetration (can't be block by shield)";
            case AttackType.Break:
                return "break (double if target have shield";
            case AttackType.Simple:
                return "simple (double if target not have a shield";
            default:
                Debug.LogWarning($"Skill {skillName} not have an attack type");
                return "default";
        }
    }

    public virtual string GetBaseValue(BaseValue _baseValue)
    {
        switch (_baseValue)
        {
            case BaseValue.CurrentHealth:
                return "current health";
            case BaseValue.BaseHealth:
                return "base health";
            case BaseValue.LostHealth:
                return "lost health";
            case BaseValue.Shield:
                return "shield";
            case BaseValue.Attack:
                return "attack";
            case BaseValue.Null:
                return 0.ToString();
            default:
                Debug.LogWarning($"Skill {skillName} not have a base value");
                return "default";
        }
    }
}
