using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "SO/EnemyStat")]
public class EnemyStatSO : ScriptableObject
{
    public float speed;
    public int health;
    public int attack;
    public float attackDis;
    public float attackSpeed;
    public float attackNum;
    public int reward;

}
