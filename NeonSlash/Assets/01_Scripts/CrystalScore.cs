using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScore : Crystal
{
    protected override void OnEaten(Player player)
    {
        SoundManager.Instance.PlayAudio(Clips.ScoreUP);
        player.starParticle.Play();
        GameManager.Instance.AddGameScore(CrystalSpawner.Instance.copyCrystalStat.crystalStat.score);
    }
}
