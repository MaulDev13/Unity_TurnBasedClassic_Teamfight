using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Shield")]
public class Skill_Shield : Skill
{
    public float repeat = 1;
    public float additionalShield = 0;
    public float baseShieldMultiplier = 0;

    public BaseValue baseValue;

    public override void Use(BattleUnit user, List<BattleUnit> targets)
    {
        base.Use(user, targets);

        if (targets.Count > 0)
        {
            foreach (BattleUnit target in targets)
            {
                target.AddShield(GetValue(user, baseValue, additionalShield, baseShieldMultiplier));
            }
        }
        else
        {
            Debug.LogWarning($"Skill {skillName} not have a target");
        }
    }

    public override void CreateDesc()
    {       
        skillDesc = $"Gain shield up to {maxTarget} {GetTarget()} with value {additionalShield}+({baseShieldMultiplier}x{GetBaseValue(baseValue)}) for {repeat} times";
    }
}
