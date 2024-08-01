using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public EnemyStatSO statSO;
    public float rotationSpeed;
    public Transform hpBar;
    Rigidbody _rigid;

    int _currentHp;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDie;

    float _notOrbTime = 0f;
    bool movable = true;
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _currentHp = statSO.health;
        TakeDamage(0);
        _notOrbTime = 1f;
        movable = true;
        OnEnableEvent?.Invoke();
    }
    void Update()
    {
        if (GameManager.Instance.isGamePlaying)
            Rotate();

        _notOrbTime -= Time.deltaTime;
        if (movable == false && _notOrbTime <= 0.9f)
            movable = true;
    }

    private void Rotate()
    {
        // 목표 방향 계산
        Vector3 direction = (Player.player.position - transform.position).normalized;

        // 목표 방향에서 Y축 회전 값만 가져오기
        Vector3 targetDirection = new Vector3(direction.x, 0, direction.z);

        // 오브젝트 회전
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isGamePlaying && _rigid.isKinematic == false && movable)
            _rigid.velocity = new Vector3(transform.forward.x * statSO.speed, _rigid.velocity.y, transform.forward.z * statSO.speed);
    }
    public void TakeDamage(int amount)
    {
        _currentHp -= amount;
        hpBar.localScale = new Vector3((float)_currentHp / statSO.health, 1f, 1f);
        if(_currentHp <= 0)
        {
            StartCoroutine(Die());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Orb") && _notOrbTime <= 0f)
        {
            TakeDamage(other.transform.root.GetComponent<PlayerSkill>().copySkillStat.skillStat.circleDamage);
            _notOrbTime = 1f;
            _rigid.AddForce((transform.position - Player.player.position).normalized * 50f, ForceMode.Impulse);
            movable = false;
        }
    }

    private IEnumerator Die()
    {
        GameManager.Instance.AddEarnMoney(statSO.reward);
        GameManager.Instance.AddGameScore(statSO.score);
        OnDie?.Invoke();
        yield return new WaitForSeconds(6f);
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
