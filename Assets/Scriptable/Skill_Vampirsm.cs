using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Vampirsm")]
public class Skill_Vampirsm : Skill
{
    public int repeat = 1;

    public float additionalDamage = 0;
    public float baseDamageMultiplier = 1f;

    public float baseHealMultiplier = 1f;

    public BaseValue attackBaseValue;


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
                            var healingPoint = user.Attack(target, GetValue(user, attackBaseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                            user.Healing(user, healingPoint * baseHealMultiplier, this);
                        }
                    }
                }
                break;
            case SkillTarget.AllAllies:
                foreach (BattleUnit target in user.myAllies)
                {
                    if (target.isAlive && target != user)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            var healingPoint = user.Attack(target, GetValue(user, attackBaseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                            user.Healing(user, healingPoint * baseHealMultiplier, this);
                        }
                    }
                }
                break;
            case SkillTarget.All:
                foreach (BattleUnit target in user.myEnemies)
                {
                    if (target.isAlive)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            var healingPoint = user.Attack(target, GetValue(user, attackBaseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                            user.Healing(user, healingPoint * baseHealMultiplier, this);
                        }
                    }
                }

                foreach (BattleUnit target in user.myAllies)
                {
                    if (target.isAlive && target != user)
                    {
                        for (int i = 0; i < repeat; i++)
                        {
                            var healingPoint = user.Attack(target, GetValue(user, attackBaseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                            user.Healing(user, healingPoint * baseHealMultiplier, this);
                        }
                    }
                }
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
                var healingPoint = user.Attack(target, GetValue(user, attackBaseValue, additionalDamage, baseDamageMultiplier), attackType, this);

                user.Healing(user, healingPoint * baseHealMultiplier, this);
            }
        }
        else
        {
            Debug.LogWarning($"Skill {skillName} not have a target");
        }
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage up to {maxTarget} {GetTarget()} with value {additionalDamage}+({baseDamageMultiplier}x{GetBaseValue(attackBaseValue)}) for {repeat} times. Heal {GetTarget(SkillTarget.Oneself)} by damage dealt";
    }
}
