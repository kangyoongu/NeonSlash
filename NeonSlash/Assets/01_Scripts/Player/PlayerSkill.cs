using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    private Player _playerCompo;

    [SerializeField] private SkillStatSO skillStatSO;
    [HideInInspector] public SkillStatSO copySkillStat;

    [Header("Orb")]
    GameObject _orbParent;
    LineRenderer[] orbTrails;
    public MeshRenderer[] orbs;
    public Color[] orbColor;

    [Header("Skill1")]
    [SerializeField] private Transform skillPivot;
    [SerializeField] private ParticleSystem _skillParticle;
    public LayerMask enemyLayer;
    float _skillCooltime = 0f;
    [SerializeField] private Image _skillImage;
    GameObject _skillImageRoot;

    [Header("Dash")]
    private Rigidbody _rigid;
    float _dashCooltime = 0f;
    [SerializeField] private Image _dashImage;
    GameObject _dashImageRoot;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        orbTrails = new LineRenderer[orbs.Length];
        _skillImageRoot = _skillImage.transform.parent.gameObject;
        _dashImageRoot = _dashImage.transform.parent.gameObject;
        for(int i = 0; i < orbs.Length; i++)
        {
            orbTrails[i] = orbs[i].gameObject.GetComponent<LineRenderer>();
        }
        _orbParent = orbs[0].transform.parent.gameObject;
        _playerCompo = GetComponent<Player>();
        ResetData();
    }

    private void OnEnable()
    {
        ItemManager.Instance.OnUpgradeSkill += ChangeStat;
        ItemManager.Instance.OnResetSkill += ResetData;
        InputManager.Instance.OnClickDash += Dash;
        InputManager.Instance.OnClickSkill += Skill;
    }

    private void OnDisable()
    {
        if (ItemManager.Instance)
        {
            ItemManager.Instance.OnUpgradeSkill -= ChangeStat;
            ItemManager.Instance.OnResetSkill -= ResetData;
        }
        if (InputManager.Instance)
        {
            InputManager.Instance.OnClickDash -= Dash;
            InputManager.Instance.OnClickSkill -= Skill;
        }
    }

    private void Update()
    {
        _skillCooltime -= Time.deltaTime;
        _skillImage.fillAmount = _skillCooltime / copySkillStat.skillStat.attackCooltime;

        _dashCooltime -= Time.deltaTime;
        _dashImage.fillAmount = _dashCooltime / copySkillStat.skillStat.dashCooltime;
    }
    private void ResetData()
    {
        copySkillStat = new SkillStatSO(skillStatSO.skillStat);
    }

    public void SetSkills()
    {
        SetOrb();
        SetSkill();
        _dashImageRoot.SetActive(copySkillStat.skillStat.unlockDash);
    }

    private void SetSkill()
    {
        _skillParticle.transform.localScale = Vector3.one * (copySkillStat.skillStat.attackDistance / 13f);
        _skillImageRoot.SetActive(copySkillStat.skillStat.unlockAttack);
    }

    private void SetOrb()
    {
        if (copySkillStat.skillStat.unlockCircle)
        {
            _orbParent.SetActive(true);
            for (int i = 0; i < orbs.Length; i++)
            {
                if (i < copySkillStat.skillStat.circleNum)
                {
                    float percent = i * Mathf.PI * 2f / copySkillStat.skillStat.circleNum;
                    float angle = 360f / copySkillStat.skillStat.circleNum;
                    orbs[i].gameObject.SetActive(true);
                    orbs[i].transform.localPosition = new Vector3(Mathf.Sin(percent) * 3f, 0f, Mathf.Cos(percent) * 3f);
                    orbs[i].transform.localRotation = Quaternion.Euler(0f, angle * i + 10, 0f);
                    orbs[i].material.color = orbColor[i];
                    orbTrails[i].startColor = orbColor[i];
                    orbTrails[i].endColor = orbColor[i];
                }
                else
                {
                    orbs[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            _orbParent.SetActive(false);
        }
    }

    private void Skill()
    {
        if (copySkillStat.skillStat.unlockAttack && _skillCooltime <= 0f && GameManager.Instance.isGamePlaying)
        {
            SoundManager.Instance.PlayAudio(Clips.Skill1, 0.45f);
            _skillParticle.Play();
            _skillCooltime = copySkillStat.skillStat.attackCooltime;
            /*List<AbstractEnemy> enemiesInSight = DetectEnemies();
            foreach (AbstractEnemy enemy in enemiesInSight)
            {
                enemy.TakeDamage(copySkillStat.skillStat.attackDamage);
            }*/
        }
    }
    

   /* List<AbstractEnemy> DetectEnemies()
    {
        List<AbstractEnemy> enemiesInSight = new List<AbstractEnemy>();

        // 구 모양으로 범위 내의 콜라이더 가져오기
        Collider[] colliders = Physics.OverlapSphere(skillPivot.position, copySkillStat.skillStat.attackDistance, enemyLayer);

        foreach (Collider col in colliders)
        {
            Transform enemyTransform = col.transform;
            Vector3 directionToEnemy = (enemyTransform.position - skillPivot.position).normalized;

            // forward 방향과의 각도 계산
            float angleToEnemy = Vector3.Angle(skillPivot.forward, directionToEnemy);

            // 각도가 detectionAngle 이내인지 확인
            if (angleToEnemy < 30f / 2f)
            {
                enemiesInSight.Add(enemyTransform.parent.GetComponent<AbstractEnemy>());
            }
        }

        return enemiesInSight;
    }*/

    private void Dash()
    {
        if (copySkillStat.skillStat.unlockDash && GameManager.Instance.isGamePlaying && _dashCooltime <= 0f)
        {
            SoundManager.Instance.PlayAudio(Clips.Dash, 1.4f);
            _dashCooltime = copySkillStat.skillStat.dashCooltime;
            _playerCompo.playerMove.movable = false;
            _rigid.AddForce(_playerCompo.playerMove.moveDirection * copySkillStat.skillStat.dashDistance * 100, ForceMode.Impulse);
            CameraController.Instance.SetDamping(Vector3.one * 2f, new Vector3(0.2f, 0.2f, 0.2f), 1f);
            StartCoroutine(Delay());
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        _playerCompo.playerMove.movable = true;
    }
    private void ChangeStat(SkillStat changeStat)
    {
        copySkillStat.skillStat.dashDistance += changeStat.dashDistance;
        copySkillStat.skillStat.dashCooltime += changeStat.dashCooltime;
        copySkillStat.skillStat.attackDamage += changeStat.attackDamage;
        copySkillStat.skillStat.attackDistance += changeStat.attackDistance;
        copySkillStat.skillStat.attackCooltime += changeStat.attackCooltime;
        copySkillStat.skillStat.circleNum += changeStat.circleNum;
        copySkillStat.skillStat.circleDamage += changeStat.circleDamage;

        if (changeStat.unlockDash) copySkillStat.skillStat.unlockDash = true;
        if (changeStat.unlockAttack) copySkillStat.skillStat.unlockAttack = true;
        if (changeStat.unlockCircle) copySkillStat.skillStat.unlockCircle = true;
        SetSkills();
    }
}
