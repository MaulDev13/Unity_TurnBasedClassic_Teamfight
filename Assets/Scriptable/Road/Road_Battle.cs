using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Road", menuName = "Road/Battle Road")]
public class Road_Battle : Road
{
    public List<Unit> enemies = new List<Unit>();

    public override void Use()
    {
        base.Use();

        ArenaGameManager.instance.BattleStart_Team(Player.instance.teammate, enemies);
    }
}
