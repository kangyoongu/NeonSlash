using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShotGun : MonoBehaviour
{
    private Player _playerCompo;
    public Transform muzzle;

    [SerializeField] private float spread;

    float _time = 0f;
    bool MouseDown => InputManager.Instance.isMouseDown;
    private void Awake()
    {
        _playerCompo = GetComponent<Player>();
    }

    private void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            _time += Time.deltaTime;
            if (MouseDown)
            {
                if (_time >= _playerCompo.playerStat.attackSpeed)
                {
                    _time = 0;
                    Shot();
                }
            }
        }
    }

    private void Shot()
    {
        for (int i = 0; i < _playerCompo.playerStat.attackNum; i++)
        {
            Transform bullet = ObjectPool.Instance.GetPooledObject("Bullet").transform;
            Vector3 spreadVec = Random.onUnitSphere * Random.Range(0f, spread);
            bullet.SetPositionAndRotation(muzzle.position, muzzle.rotation * Quaternion.Euler(spreadVec));
            bullet.GetComponent<Bullet>().Init(_playerCompo.playerStat.attackDis, _playerCompo.playerStat.attack);
        }
    }
}
