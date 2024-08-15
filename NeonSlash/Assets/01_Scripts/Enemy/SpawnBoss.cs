using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SpawnBoss : MonoBehaviour
{
    public float minTime = 30f;
    public float maxTime = 60f;
    float time;
    public Image bossSpawnUI;
    public Boss boss;
    bool spawnTrigger = false;
    bool bossIsDie = true;
    [SerializeField] private BossSO[] progressBossSO;
    int soIndex = 0;
    private void OnDisable()
    {
        InputManager.Instance.OnClickF -= Spawn;
    }
    void Update()
    {
        if (GameManager.Instance.isGamePlaying && bossIsDie)
        {
            time += Time.deltaTime;
            bossSpawnUI.fillAmount = 1f - time / 30f;
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
        bossIsDie = false;
        InputManager.Instance.OnClickF -= Spawn;
        time = 0;
        spawnTrigger = false;
        SoundManager.Instance.PlayAudio(Clips.BossSpawn);
        int index = progressBossSO.Length <= soIndex ? progressBossSO.Length - 1 : soIndex;
        boss.Go(progressBossSO[index]);
        soIndex++;
        Vector2 offset = Random.insideUnitCircle.normalized;
        Vector3 vec = Player.player.position + new Vector3(offset.x, 0f, offset.y) * 50f;
        vec.y = 0f;
        boss.transform.position = vec;
        bossSpawnUI.fillAmount = 1f;
    }
    public void BossIsDie()
    {
        bossIsDie = true;
    }
}
