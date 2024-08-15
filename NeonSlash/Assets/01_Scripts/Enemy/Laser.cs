using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Boss boss;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.root.GetComponent<Player>();
            player.TakeDamage(boss.currentBossSO.laserDamage * Time.deltaTime);
        }
    }
}
