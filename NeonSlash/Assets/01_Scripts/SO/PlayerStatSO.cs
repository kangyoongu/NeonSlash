using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "SO/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    public PlayerStat playerStat;
    public PlayerStatSO(PlayerStat data)
    {
        playerStat.speed = data.speed;
        playerStat.fieldOfView = data.fieldOfView;
        playerStat.attack = data.attack;
        playerStat.attackDis = data.attackDis;
        playerStat.health = data.health;
        playerStat.attackSpeed = data.attackSpeed;
        playerStat.attackNum = data.attackNum;
    }
}

[Serializable]
public struct PlayerStat
{
    public float speed;
    public float fieldOfView;
    public int attack;
    public int attackDis;
    public int health;
    public float attackSpeed;
    public float attackNum;
}