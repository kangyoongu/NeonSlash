using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "SO/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    public float speed;
    public float fieldOfView;
    public int attack;
    public int attackDis;
    public int health;
    public float attackSpeed;
    public float attackNum;
    public PlayerStatSO(PlayerStatSO data)
    {
        speed = data.speed;
        fieldOfView = data.fieldOfView;
        attack = data.attack;
        attackDis = data.attackDis;
        health = data.health;
        attackSpeed = data.attackSpeed;
        attackNum = data.attackNum;
    }
}
