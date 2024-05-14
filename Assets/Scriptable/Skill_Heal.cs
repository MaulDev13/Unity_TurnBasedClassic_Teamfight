using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Heal")]
public class Skill_Heal : Skill
{
    public float repeat = 1;
    public float additionalHeal = 0;
    public float baseHealMultiplier = 1f;

    public BaseValue baseValue;

    public override void Use(BattleUnit user, List<BattleUnit> targets)
    {
        base.Use(user, targets);

        if (targets.Count > 0)
        {
            foreach (BattleUnit target in targets)
            {
                for (int i = 0; i < repeat; i++)
                {
                    user.Healing(target, GetValue(user, baseValue, additionalHeal, baseHealMultiplier));
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
        skillDesc = $"Heal up to {maxTarget} {GetTarget()} with value {additionalHeal}+({baseHealMultiplier}x{GetBaseValue(baseValue)}) for {repeat} times";
    }
}
