using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Attack")]
public class Skill_BasicAtk : Skill
{
    public float repeat = 1;
    public float additionalDamage = 0;
    public float baseDamageMultiplier = 1f;

    public BaseValue baseValue;

    public AttackType attackType;

    public override void Use(BattleUnit user, List<BattleUnit> targets)
    {
        base.Use(user, targets);

        if(targets.Count > 0)
        {
            foreach (BattleUnit target in targets)
            {
                for (int i = 0; i < repeat; i++)
                {
                    user.Attack(target, GetValue(user, baseValue, additionalDamage, baseDamageMultiplier), attackType);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Skill {skillName} not have a target");
        }
    }

    public override void CreateDesc()
    {
        skillDesc = $"Deal damage up to {maxTarget} {GetTarget()} with value {additionalDamage}+({baseDamageMultiplier}x{GetBaseValue(baseValue)}) for {repeat} times. Attack type is {GetAttackType(attackType)}";
    }
}
