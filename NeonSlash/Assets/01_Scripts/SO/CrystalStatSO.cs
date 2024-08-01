using System;
using UnityEngine;


[CreateAssetMenu(fileName = "CrystalStatSO", menuName = "SO/CrystalStatSO")]
public class CrystalStatSO : ScriptableObject
{
    public CrystalStat crystalStat;

    public CrystalStatSO(CrystalStat data)
    {
        crystalStat.unlockHeal = data.unlockHeal;
        crystalStat.heal = data.heal;
        crystalStat.healPercent = data.healPercent;

        crystalStat.unlockScore = data.unlockScore;
        crystalStat.score = data.score;
        crystalStat.scorePercent = data.scorePercent;

        crystalStat.unlockInv = data.unlockInv;
        crystalStat.invTime = data.invTime;
        crystalStat.invPercent = data.invPercent;
    }
}

[Serializable]
public struct CrystalStat
{
    public bool unlockHeal;
    public int heal;
    public float healPercent;

    public bool unlockScore;
    public int score;
    public float scorePercent;

    public bool unlockInv;
    public float invTime;
    public float invPercent;
}
