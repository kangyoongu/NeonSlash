using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalInv : Crystal
{
    protected override void OnEaten(Player player)
    {
        player.StartInv(CrystalSpawner.Instance.copyCrystalStat.crystalStat.invTime);
    }
}
