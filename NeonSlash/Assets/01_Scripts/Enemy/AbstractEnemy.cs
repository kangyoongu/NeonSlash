using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractEnemy : MonoBehaviour
{
    public Transform hpBar;
    protected Rigidbody _rigid;

    protected int _currentHp;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDie;

    protected void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }
    protected virtual void OnEnable()
    {
        _currentHp = GetHealth();
        TakeDamage(0);
        OnEnableEvent?.Invoke();
    }
    public void TakeDamage(int amount)
    {
        _currentHp -= amount;
        hpBar.localScale = new Vector3((float)_currentHp / GetHealth(), 1f, 1f);
        if (_currentHp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    protected virtual IEnumerator Die()
    {
        OnDie?.Invoke();
        yield return new WaitForSeconds(7f);
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    protected abstract int GetHealth();
}
