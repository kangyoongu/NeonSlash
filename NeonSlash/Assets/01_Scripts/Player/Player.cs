using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Transform player;

    [HideInInspector] public Transform visual;

    [SerializeField] private PlayerStatSO _playerStatSO;
    [HideInInspector] public PlayerStatSO playerStat;

    int currentHp;
    private void Awake()
    {
        player = transform;
        visual = transform.Find("PlayerVisual");
        playerStat = new PlayerStatSO(_playerStatSO);
    }
    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStart;
    }

    private void GameStart()
    {
        CameraController.Instance.SetFOV(playerStat.fieldOfView);
        currentHp = playerStat.health;
        UIManager.Instance.SetHP(currentHp, playerStat.health);
    }
    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        UIManager.Instance.SetHP(currentHp, playerStat.health);
        if(currentHp <= 0)
        {
            currentHp = 0;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        GameManager.Instance.isGamePlaying = false;
        yield return new WaitForSeconds(2f);
        UIManager.Instance.EndingUIIn(false);
    }
}
