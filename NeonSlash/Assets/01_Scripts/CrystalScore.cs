using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScore : Crystal
{
    protected override void OnEaten(Player player)
    {
        GameManager.Instance.AddGameScore(CrystalSpawner.Instance.copyCrystalStat.crystalStat.score);
    }
}
