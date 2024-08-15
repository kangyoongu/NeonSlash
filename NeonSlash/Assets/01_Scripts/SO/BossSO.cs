using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss", menuName = "SO/Boss")]
public class BossSO : ScriptableObject
{
    public int health;
    public int reward;
    public int score;
    public float speed;

    public float patternDelay;

    [Header("Dash")]
    public int dashDamage;
    public float dashSpeed;

    [Header("Cross")]
    public int crossDamage;
    public float shotDelay;
    public float shotCount;
    public float bulletLife;
    public float oneShotTurn;

    [Header("Laser")]
    public int laserDamage;
    public int laserTime;
    public int laserDelay;
    public int laserCount;
}
