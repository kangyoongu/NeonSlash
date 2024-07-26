using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStatSO statSO;
    public float rotationSpeed;
    public Transform hpBar;
    Rigidbody _rigid;

    int _currentHp;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _currentHp = statSO.health;
    }
    void Update()
    {
        if (GameManager.Instance.isGamePlaying)
            Rotate();
    }

    private void Rotate()
    {
        // 목표 방향 계산
        Vector3 direction = (Player.player.position - transform.position).normalized;

        // 목표 방향에서 Y축 회전 값만 가져오기
        Vector3 targetDirection = new Vector3(direction.x, 0, direction.z);

        // 오브젝트 회전
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isGamePlaying)
            _rigid.velocity = transform.forward * statSO.speed;
    }
    public void TakeDamage(int amount)
    {
        _currentHp -= amount;
        hpBar.localScale = new Vector3((float)_currentHp / statSO.health, 1f, 1f);
        if(_currentHp <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        ObjectPool.Instance.ReturnToPool(gameObject);
        yield return null;
    }
}
