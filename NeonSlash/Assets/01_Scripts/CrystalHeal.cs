using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHeal : Crystal
{
    protected override void OnEaten(Player player)
    {
        player.TakeDamage(-CrystalSpawner.Instance.copyCrystalStat.crystalStat.heal);
    }
}
