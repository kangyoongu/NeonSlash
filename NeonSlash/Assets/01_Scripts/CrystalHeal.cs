using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalHeal : Crystal
{
    protected override void OnEaten(Player player)
    {
        SoundManager.Instance.PlayAudio(Clips.Healing);
        player.hpParticle.Play();
        player.TakeDamage(-CrystalSpawner.Instance.copyCrystalStat.crystalStat.heal);
    }
}
