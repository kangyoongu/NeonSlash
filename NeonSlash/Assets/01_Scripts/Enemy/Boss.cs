using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : AbstractEnemy
{
    [HideInInspector] public BossSO currentBossSO;
    public Player player;
    [HideInInspector] public AudioSource audioSource;
    public Transform bulletTrm;

    bool _doNothing = true;
    bool dash = false;
    float _patternTime = 0f;

    [SerializeField] List<Transform> _warning;
    [SerializeField] List<Transform> _laser;

    Queue<Transform> _warningQ = new();
    Queue<Transform> _laserQ = new();
    private Tweener _warningTween;
    private Tweener _laserTween;

    Vector3 direction;
    string[] patterns = { "Dash", "Cross", "Laser" };
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        for(int i = 0; i < _warning.Count; i++)
        {
            _warningQ.Enqueue(_warning[i]);
            _laserQ.Enqueue(_laser[i]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb"))
        {
            SoundManager.Instance.PlayAudio(Clips.OrbHit);
            TakeDamage(other.transform.root.GetComponent<PlayerSkill>().copySkillStat.skillStat.circleDamage);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && dash == true)
        {
            player.TakeDamage(currentBossSO.dashDamage * Time.deltaTime);
        }
    }

    internal void Go(BossSO bossSO)
    {
        currentBossSO = bossSO;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        direction = new Vector3(Player.player.position.x - transform.position.x, 0, Player.player.position.z - transform.position.z);
        direction.Normalize();
        if (_doNothing && GameManager.Instance.isGamePlaying)
        {
            _patternTime += Time.deltaTime;
            if(_patternTime >= currentBossSO.patternDelay)
            {
                _patternTime = 0;
                _doNothing = false;

                StartCoroutine(patterns[Random.Range(0, 3)]);
            }
            else
            {

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
                transform.Translate(Vector3.forward * Time.deltaTime * currentBossSO.speed);
            }
        }
        
    }
    private IEnumerator Dash()
    {
        dash = true;

        float delayTime = 0f;//플레이어 바라보기
        while (delayTime < 0.8f)
        {
            yield return null;
            delayTime += Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f * Time.deltaTime);
        }

        audioSource.PlayOneShot(SoundManager.Instance.clips3D.bossDash, 5f);
        Vector3 targetPos = Player.player.position;
        targetPos.y = 0f;
        float delay = Vector3.Distance(transform.position, Player.player.position) * currentBossSO.dashSpeed;
        transform.DOMove(targetPos + direction, delay)
            .SetEase(Ease.Linear);
        yield return new WaitForSeconds(delay);

        //dashParticle.Stop();
        _doNothing = true;
        dash = false;
    }
    private IEnumerator Cross()
    {
        for (int i = 0; i < currentBossSO.shotCount; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if(!GameManager.Instance.isGamePlaying) yield break;

                audioSource.PlayOneShot(SoundManager.Instance.clips3D.enemyShot, 1f);
                Transform bullet = ObjectPool.Instance.GetPooledObject("EnemyBullet").transform;
                bullet.SetPositionAndRotation(bulletTrm.position, bulletTrm.rotation * Quaternion.Euler(0f, 0f, 90f * j));
                bullet.GetComponent<Bullet>().Init(currentBossSO.bulletLife, currentBossSO.crossDamage);
                bulletTrm.Rotate(Vector3.forward * currentBossSO.oneShotTurn);
            }
            yield return new WaitForSeconds(currentBossSO.shotDelay);
        }
        _doNothing = true;
    }
    private IEnumerator Laser()
    {
        for (int i = 0; i < currentBossSO.laserCount; i++)
        {
            if (!GameManager.Instance.isGamePlaying) yield break;

            Transform warning = _warningQ.Dequeue();
            Transform laser = _laserQ.Dequeue();

            _warningTween = warning.DOScale(Vector3.one, 0.6f);//레이저 위치 알려줌
            yield return new WaitForSeconds(0.6f);

            audioSource.PlayOneShot(SoundManager.Instance.clips3D.bossLaser, 2f);

            _laserTween = laser.DOScale(Vector3.one, 0.6f);//진짜 레이저 발사
            yield return new WaitForSeconds(currentBossSO.laserTime);
            
            _warningTween = warning.DOScale(Vector3.zero, 0.4f);//레이저 없애기
            _laserTween = laser.DOScale(Vector3.zero, 0.4f);
            
            yield return new WaitForSeconds(0.4f);
            
            _laserQ.Enqueue(laser);
            _warningQ.Enqueue(warning);
            
            float delayTime = 0f;//플레이어 바라보기
            while(delayTime < currentBossSO.laserDelay)
            {
                yield return null;
                delayTime += Time.deltaTime;

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4f * Time.deltaTime);
            }
        }
        _doNothing = true;
    }
    protected override IEnumerator Die()
    {
        audioSource.PlayOneShot(SoundManager.Instance.clips3D.enemyDie, 6f);
        GameManager.Instance.AddEarnMoney(currentBossSO.reward);
        GameManager.Instance.AddGameScore(currentBossSO.score);
        GameManager.Instance.AddPoint(1);

        _warningQ = new();
        _laserQ = new();
        for (int i = 0; i < _warning.Count; i++)
        {
            _warningQ.Enqueue(_warning[i]);
            _laserQ.Enqueue(_laser[i]);
        }

        StopCoroutine("Laser");
        StopCoroutine("Cross");
        StopCoroutine("Dash");
        _doNothing = false;
        _patternTime = 0;
        if (_warningTween != null)
        {
            _warningTween.Kill();
            _laserTween.Kill();
        }
        for (int i = 0; i < _warning.Count; i++)
        {
            _warning[i].localScale = Vector3.zero;
            _laser[i].localScale = Vector3.zero;
        }
        OnDie?.Invoke();
        yield return new WaitForSeconds(4.5f);
        gameObject.SetActive(false);
        _doNothing = true;
    }
    protected override int GetHealth()
    {
        return currentBossSO.health;
    }
}