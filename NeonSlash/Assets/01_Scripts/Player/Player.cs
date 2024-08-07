using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Transform player;

    [HideInInspector] public Transform visual;

    [SerializeField] private PlayerStatSO _playerStatSO;
    [HideInInspector] public PlayerStatSO copyPlayerStat;

    PlayerSkill _playerSkill;
    [HideInInspector] public PlayerMovement playerMove;
    public ParticleSystem hpParticle;
    public ParticleSystem starParticle;
    int _currentHp;

    bool _inv = false;
    public Material _invMaterial;
    float _invTime = 0;
    private void Awake()
    {
        _playerSkill = GetComponent<PlayerSkill>();
        playerMove = GetComponent<PlayerMovement>();
        player = transform;
        visual = transform.Find("PlayerVisual");
        ResetData();
        _invMaterial.SetFloat("_Lerp", 0f);
    }

    private void ResetData()
    {
        copyPlayerStat = new PlayerStatSO(_playerStatSO.playerStat);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += GameStart;
        ItemManager.Instance.OnUpgradePlayer += ChangeStat;
        ItemManager.Instance.OnResetPlayer += ResetData;
    }

    private void OnDisable()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnGameStart -= GameStart;
        if (ItemManager.Instance != null)
        {
            ItemManager.Instance.OnUpgradePlayer -= ChangeStat;
            ItemManager.Instance.OnResetPlayer -= ResetData;
        }
    }

    private void GameStart()
    {
        CameraController.Instance.SetFOV(copyPlayerStat.playerStat.fieldOfView);
        _currentHp = copyPlayerStat.playerStat.health;
        UIManager.Instance.SetHP(_currentHp, copyPlayerStat.playerStat.health);
        _playerSkill.GameStart();
    }
    public void TakeDamage(int amount)
    {
        if (!_inv || amount < 0)
        {
            SoundManager.Instance.PlayAudio(Clips.PlayerHit, 0.2f);
            _currentHp -= amount;
            if (_currentHp <= 0)
            {
                _currentHp = 0;
                StartCoroutine(Die());
            }
            else if(_currentHp > copyPlayerStat.playerStat.health)
            {
                _currentHp = copyPlayerStat.playerStat.health;
            }
            UIManager.Instance.SetHP(_currentHp, copyPlayerStat.playerStat.health);
        }
    }
    public void StartInv(float time)
    {
        _inv = true;
        _invMaterial.DOFloat(1f, "_Lerp", 1f);
        _invTime += time;

    }
    private void Update()
    {
        if(_invTime > 0f)
        {
            _invTime -= Time.deltaTime;
        }
        else if(_invTime <= 0f && _inv)
        {
            _invTime = 0;
            _inv = false;
            _invMaterial.DOFloat(0f, "_Lerp", 1f);
        }
    }
    private IEnumerator Die()
    {
        SoundManager.Instance.PlayAudio(Clips.PlayerDie, 0.3f);
        GameManager.Instance.isGamePlaying = false;
        yield return new WaitForSeconds(2f);
        GameManager.Instance.ApplyScore();
        UIManager.Instance.EndingUIIn(false);
    }

    private void ChangeStat(PlayerStat changeStat)
    {
        copyPlayerStat.playerStat.speed       += changeStat.speed;
        copyPlayerStat.playerStat.fieldOfView += changeStat.fieldOfView;
        copyPlayerStat.playerStat.attack      += changeStat.attack;
        copyPlayerStat.playerStat.attackDis   += changeStat.attackDis;
        copyPlayerStat.playerStat.health      += changeStat.health;
        copyPlayerStat.playerStat.attackSpeed += changeStat.attackSpeed;
        copyPlayerStat.playerStat.attackNum   += changeStat.attackNum;
    }
}
