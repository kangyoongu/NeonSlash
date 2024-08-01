using System;
using UnityEngine;


[CreateAssetMenu(fileName = "SkillStat", menuName = "SO/Skill")]
public class SkillStatSO : ScriptableObject
{
    public SkillStat skillStat;

    public SkillStatSO(SkillStat data)
    {
        skillStat.unlockDash = data.unlockDash;
        skillStat.dashDistance = data.dashDistance;
        skillStat.dashCooltime = data.dashCooltime;
        skillStat.unlockAttack = data.unlockAttack;
        skillStat.attackDamage = data.attackDamage;
        skillStat.attackDistance = data.attackDistance;
        skillStat.attackCooltime = data.attackCooltime;
        skillStat.unlockCircle = data.unlockCircle;
        skillStat.circleNum = data.circleNum;
        skillStat.circleDamage = data.circleDamage;
    }
}

[Serializable]
public struct SkillStat
{
    public bool unlockDash;
    public float dashDistance;
    public float dashCooltime;

    public bool unlockAttack;
    public int attackDamage;
    public float attackDistance;
    public float attackCooltime;

    public bool unlockCircle;
    public int circleNum;
    public int circleDamage;
}
