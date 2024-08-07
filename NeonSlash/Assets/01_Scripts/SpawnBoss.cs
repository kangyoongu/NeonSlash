using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBoss : MonoBehaviour
{
    public float minTime = 30f;
    public float maxTime = 60f;
    float time;
    public Image bossSpawnUI;

    bool spawnTrigger = false;
    private void OnDisable()
    {
        InputManager.Instance.OnClickF -= Spawn;
    }
    void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            time += Time.deltaTime;
            bossSpawnUI.fillAmount = time / 30f;
            if (time >= minTime)
            {
                if (spawnTrigger == false)
                {
                    InputManager.Instance.OnClickF += Spawn;
                    spawnTrigger = true;
                }
                if(time >= maxTime)
                {
                    Spawn();
                }
            }
        }
    }

    private void Spawn()
    {
        InputManager.Instance.OnClickF -= Spawn;
        time = 0;
        spawnTrigger = false;
        print("print");
    }
}
