using UnityEngine;
public enum EnemyType
{
    Normal,
    Strong,
    Shotgun,
    Faster,
    Damage,
    Killer
}
[CreateAssetMenu(fileName = "UpgradeEnemy", menuName = "SO/Category/Enemy")]
public class UpgradeEnemySO : ScriptableObject
{
    public EnemyType unlockEnemy; 
}
