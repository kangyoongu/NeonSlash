using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        AbstractEnemy enemy;
        if (other.transform.parent.TryGetComponent(out enemy))
        {
            enemy.OnHitOrb(transform.root);
        }
    }
}
