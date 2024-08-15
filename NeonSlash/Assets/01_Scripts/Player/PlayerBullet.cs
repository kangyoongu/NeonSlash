using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.transform.parent.GetComponent<AbstractEnemy>().TakeDamage(damage);
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
}
