using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Shield")]
public class Skill_DoubleEdge : Skill
{
    public float repeat = 1;
    public float additionalDamage = 0;
    public float baseDamageMultiplier = 1f;

    public BaseValue baseValue;

    public AttackType attackType;

    public override void Use(BattleUnit user)
    {
        base.Use(user);

        switch (target)
        {
            case SkillTarget.AllEnemies:
                foreach (BattleUnit target in user.myEnemies)
                {
                    if (target.isAlive)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            user.Attack(target, GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
                        }
                    }
                }

                user.TakeDamage(GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                break;
            case SkillTarget.AllAllies:
                foreach (BattleUnit target in user.myAllies)
                {
                    if (target.isAlive && target != user)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            user.Attack(target, GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
                        }
                    }
                }

                user.TakeDamage(GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                break;
            case SkillTarget.All:
                foreach (BattleUnit target in user.myEnemies)
                {
                    if (target.isAlive)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            user.Attack(target, GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
                        }
                    }
                }

                foreach (BattleUnit target in user.myAllies)
                {
                    if (target.isAlive && target != user)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            user.Attack(target, GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
                        }
                    }
                }

                user.TakeDamage(GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
                break;
            case SkillTarget.Null:
                break;
            default:
                break;
        }
    }

    public override void Use(BattleUnit user, List<BattleUnit> targets)
    {
        base.Use(user, targets);

        if (targets.Count > 0)
        {
            foreach (BattleUnit target in targets)
            {
                for (int i = 0; i < repeat; i++)
                {
                    user.Attack(target, GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
                }
            }

            user.TakeDamage(GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType, this);
        }
        else
        {
            Debug.LogWarning($"Skill {skillName} not have a target");
        }
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage up to {maxTarget} {GetTarget()} with value {additionalDamage}+({baseDamageMultiplier}x{GetBaseValue(baseValue)}) for {repeat} times. Deal damage to {GetTarget(SkillTarget.Oneself)}. Attack type is {GetAttackType(attackType)}";
    }
}
