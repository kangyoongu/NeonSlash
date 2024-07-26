using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.root.GetComponent<Player>().TakeDamage(damage);
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
