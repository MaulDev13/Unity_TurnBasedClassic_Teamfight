using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Escape")]
public class Skill_Escape : Skill
{
    public float escapeChance = 100f;

    public override void Use(BattleUnit user)
    {
        base.Use(user);

        Escape();
    }
    public override void Use(BattleUnit user, List<BattleUnit> targets)
    {
        base.Use(user, targets);

        Escape();
    }

    private void Escape()
    {
        float randomChance = Random.Range(0f, 100f);

        if (randomChance <= escapeChance)
        {
            LocalManager_Arena.instance.Forfeit();
            return;
        }    
    }

    public override void CreateDesc()
    {
        skillDesc = $"{escapeChance}% chance to escape the battle";
    }
}
