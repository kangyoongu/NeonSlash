using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crystal : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine("Return");
    }
    IEnumerator Return()
    {
        yield return new WaitForSeconds(20f);
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
    private void OnDisable()
    {
        StopCoroutine("Return");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnEaten(other.transform.root.GetComponent<Player>());
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }
    protected abstract void OnEaten(Player player);
}
