using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Attack and Shield")]
public class Skill_AttackAndDefense : Skill
{
    public int repeat = 1;

    public float additionalShield = 0;
    public float baseShieldMultiplier = 0;

    public float additionalDamage = 0;
    public float baseDamageMultiplier = 1f;

    public BaseValue attackBaseValue;
    public BaseValue shieldBaseValue;


    public AttackType attackType;

    public override void Use(BattleUnit user, List<BattleUnit> targets)
    {
        base.Use(user, targets);

        if (targets.Count > 0)
        {
            foreach (BattleUnit target in targets)
            {
                user.Attack(target, GetValue(user, attackBaseValue, additionalDamage, baseDamageMultiplier), attackType);
            }

            user.AddShield(GetValue(user, shieldBaseValue, additionalShield, baseShieldMultiplier));
        }
        else
        {
            Debug.LogWarning($"Skill {skillName} not have a target");
        }
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage up to {maxTarget} {GetTarget()} with value {additionalDamage}+({baseDamageMultiplier}x{GetBaseValue(attackBaseValue)}) for {repeat} times. Attack type is {GetAttackType(attackType)}. Gain shield for oneself with value {additionalShield}+({baseShieldMultiplier}x{GetBaseValue(shieldBaseValue)})";
    }
}

