using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    private float _lifeTime;
    protected int damage;
    [SerializeField] private int _speed = 70;
    float time = 0;
    public virtual void Init(float lifeTime, int damage)
    {
        _lifeTime = lifeTime;
        this.damage = damage;
    }
    void Update()
    {
        time += Time.deltaTime;
        if(time >= _lifeTime)
        {
            time = 0;
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
    }
}
