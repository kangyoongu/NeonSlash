using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float[] phase;
    float time = 0;
    int currentPhase = 0;
    void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            time += Time.deltaTime;
            if(time >= phase[currentPhase])
            {
                time = 0;
                currentPhase++;
                for(int i = 0; i < 10; i++)
                {
                    SpawnEnemy("NormalEnemy");
                }
            }
        }
        else
        {
            time = 0;
        }
    }

    private void SpawnEnemy(string name)
    {
        GameObject enemy = ObjectPool.Instance.GetPooledObject(name);
        Vector2 offset = Random.insideUnitCircle.normalized;
        enemy.transform.position = Player.player.position + new Vector3(offset.x, 0f, offset.y) * 50f;
    }
}
