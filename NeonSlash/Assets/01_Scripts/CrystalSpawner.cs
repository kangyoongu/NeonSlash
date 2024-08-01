using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalSpawner : SingleTon<CrystalSpawner>
{
    [SerializeField] private CrystalStatSO crystalStatSO;
     public CrystalStatSO copyCrystalStat;

    [SerializeField] private float _interval = 5f;
    float _time = 0;

    private void Awake()
    {
        ResetData();
    }

    private void OnEnable()
    {
        ItemManager.Instance.OnUpgradeCrystal += ChangeStat;
        ItemManager.Instance.OnResetSkill += ResetData;
    }

    private void OnDisable()
    {
        if (ItemManager.Instance)
        {
            ItemManager.Instance.OnUpgradeCrystal -= ChangeStat;
            ItemManager.Instance.OnResetSkill -= ResetData;
        }
    }
    private void ResetData()
    {
        copyCrystalStat = new CrystalStatSO(crystalStatSO.crystalStat);
    }
    void Update()
    {
        if (GameManager.Instance.isGamePlaying)
        {
            _time += Time.deltaTime;
            if(_time >= _interval)
            {
                _time = 0;
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        SpawnCrystal(copyCrystalStat.crystalStat.healPercent, copyCrystalStat.crystalStat.unlockHeal, "HealCrystal");
        SpawnCrystal(copyCrystalStat.crystalStat.invPercent, copyCrystalStat.crystalStat.unlockInv, "InvCrystal");
        SpawnCrystal(copyCrystalStat.crystalStat.scorePercent, copyCrystalStat.crystalStat.unlockScore, "ScoreCrystal");
    }

    private void SpawnCrystal(float percent, bool unlock, string name)
    {
        if (Core.RandomPercent(percent) && unlock)
        {
            GameObject crystal = ObjectPool.Instance.GetPooledObject(name);
            Vector3 offset = Random.insideUnitCircle.normalized * 35f;
            crystal.transform.position = Player.player.position + new Vector3(offset.x, 0f, offset.y);
        }
    }

    private void ChangeStat(CrystalStat changeStat)
    {
        copyCrystalStat.crystalStat.heal += changeStat.heal;
        copyCrystalStat.crystalStat.healPercent += changeStat.healPercent;
        copyCrystalStat.crystalStat.score += changeStat.score;
        copyCrystalStat.crystalStat.scorePercent += changeStat.scorePercent;
        copyCrystalStat.crystalStat.invTime += changeStat.invTime;
        copyCrystalStat.crystalStat.invPercent += changeStat.invPercent;

        if (changeStat.unlockHeal)  copyCrystalStat.crystalStat.unlockHeal = true;
        if (changeStat.unlockScore) copyCrystalStat.crystalStat.unlockScore = true;
        if (changeStat.unlockInv)   copyCrystalStat.crystalStat.unlockInv = true;
    }
}
