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
                if (_time >= _playerCompo.copyPlayerStat.playerStat.attackSpeed)
                {
                    _time = 0;
                    Shot();
                }
            }
        }
    }

    private void Shot()
    {
        SoundManager.Instance.PlayAudio(Clips.PlayerShot, 0.15f);
        for (int i = 0; i < _playerCompo.copyPlayerStat.playerStat.attackNum; i++)
        {
            Transform bullet = ObjectPool.Instance.GetPooledObject("Bullet").transform;
            Vector3 spreadVec = Random.onUnitSphere * Random.Range(0f, spread) * Mathf.InverseLerp(1f, 6f, _playerCompo.copyPlayerStat.playerStat.attackNum);//���߿� �ִ� attackNum���� ����
            bullet.SetPositionAndRotation(muzzle.position, muzzle.rotation * Quaternion.Euler(spreadVec));
            bullet.GetComponent<Bullet>().Init(_playerCompo.copyPlayerStat.playerStat.attackDis, _playerCompo.copyPlayerStat.playerStat.attack);
        }
    }
}
