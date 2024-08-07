using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalInv : Crystal
{
    protected override void OnEaten(Player player)
    {
        SoundManager.Instance.PlayAudio(Clips.Inv);
        player.StartInv(CrystalSpawner.Instance.copyCrystalStat.crystalStat.invTime);
    }
}
