using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private Enemy _enemyCompo;
    public Transform muzzle;

    [SerializeField] private float spread;

    float _time = 0f;
    private void Awake()
    {
        _enemyCompo = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            _time += Time.deltaTime;
            if (_time >= _enemyCompo.statSO.attackSpeed)
            {
                _time = 0;
                Shot();
            }
        }
    }
    private void OnDisable()
    {
        _time = 0;
    }
    private void Shot()
    {
        _enemyCompo.audioSource.PlayOneShot(SoundManager.Instance.clips3D.enemyShot, 0.8f);
        for (int i = 0; i < _enemyCompo.statSO.attackNum; i++)
        {
            Transform bullet = ObjectPool.Instance.GetPooledObject("EnemyBullet").transform;
            Vector3 spreadVec = Random.onUnitSphere * Random.Range(0f, spread) * Mathf.InverseLerp(1f, 5f, _enemyCompo.statSO.attackNum);
            bullet.SetPositionAndRotation(muzzle.position, muzzle.rotation * Quaternion.Euler(spreadVec));
            bullet.GetComponent<Bullet>().Init(_enemyCompo.statSO.attackDis, _enemyCompo.statSO.attack);
        }
    }
}
