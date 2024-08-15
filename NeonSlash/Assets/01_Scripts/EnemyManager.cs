using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public struct PhaseData
{
    [Tooltip("���̺� ���ۋ� ������ �� Ÿ�԰� ����")]
    public SpawnData[] spawnData;

    [Tooltip("�� ���̺�κ��� ���� �� �� ���̺갡 ���۵Ǵ°�")]
    public float time;

    [Tooltip("�׽� �����ϴ� ��")]
    public DefaltSpawnData[] defaltSpawnData;
}
[Serializable]
public struct SpawnData
{
    public EnemyType enemyType;
    public int count;
}

[Serializable]
public struct DefaltSpawnData
{
    [Tooltip("� ���� ��������")]
    public EnemyType enemyType;

    [Tooltip("�� ���� ���ʸ��� ��������")]
    public float time;
}


public class EnemyManager : SingleTon<EnemyManager>
{
    public List<PhaseData> spawnDatas = new List<PhaseData>();

    float _time = 0;
    int _currentPhase = 0;

    private Dictionary<EnemyType, bool> _unlockEnemyDic = new Dictionary<EnemyType, bool>();

    public void SetUnlockData()
    {
        ResetUnlockData();
        for(int i = 1; i < _unlockEnemyDic.Count; i++)
        {
            _unlockEnemyDic[ItemManager.Instance.categoryCtrls[ItemCategory.Enemy][i - 1].itemSO.upgradePerLevel[0].enemy.unlockEnemy] = Core.IsBitSet(JsonManager.Instance._gameData.enemyBool, i-1);
        }
        _unlockEnemyDic[EnemyType.Normal] = true;
    }
    public void ResetUnlockData()
    {
        EnemyType[] enemys = (EnemyType[])Enum.GetValues(typeof(EnemyType));
        _unlockEnemyDic = new Dictionary<EnemyType, bool>();
        foreach (EnemyType enemy in enemys)
        {
            _unlockEnemyDic.Add(enemy, false);
        }
        _unlockEnemyDic[EnemyType.Normal] = true;
    }
    public void UnlockEnemy(EnemyType type)
    {
        _unlockEnemyDic[type] = true;
    }
    void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            _time += Time.deltaTime;
            if(spawnDatas.Count > _currentPhase && _time >= spawnDatas[_currentPhase].time)
            {
                for(int i = 0; i < spawnDatas[_currentPhase].spawnData.Length; i++)
                {
                    for (int j = 0; j < spawnDatas[_currentPhase].spawnData[i].count; j++)
                    {
                        if (_unlockEnemyDic[spawnDatas[_currentPhase].spawnData[i].enemyType]) 
                            SpawnEnemy(spawnDatas[_currentPhase].spawnData[i].enemyType);
                    }
                }
                for (int j = 0; j < spawnDatas[_currentPhase].defaltSpawnData.Length; j++)
                {
                    if (_unlockEnemyDic[spawnDatas[_currentPhase].defaltSpawnData[j].enemyType])
                        StartCoroutine(SpawnDelay(_currentPhase, j));
                }

                _time = 0;
                _currentPhase++;
            }
        }
        else
        {
            _time = 0;
        }
    }
    IEnumerator SpawnDelay(int i, int j)
    {
        while (_currentPhase <= i+1)
        {
            yield return new WaitForSeconds(spawnDatas[i].defaltSpawnData[j].time);
            SpawnEnemy(spawnDatas[i].defaltSpawnData[j].enemyType);
        }
    }
    private void SpawnEnemy(EnemyType type)
    {
        GameObject enemy = ObjectPool.Instance.GetPooledObject(type.ToString() + "Enemy");
        Vector2 offset = Random.insideUnitCircle.normalized;
        enemy.transform.position = Player.player.position + new Vector3(offset.x, 0f, offset.y) * 50f;
    }

}
