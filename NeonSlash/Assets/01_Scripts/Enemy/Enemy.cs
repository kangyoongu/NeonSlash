using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : AbstractEnemy
{
    public EnemyStatSO statSO;
    public float rotationSpeed;

    float _notOrbTime = 0f;
    bool movable = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        _notOrbTime = 1f;
        movable = true;
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
        // ��ǥ ���� ���
        Vector3 direction = (Player.player.position - transform.position).normalized;

        // ��ǥ ���⿡�� Y�� ȸ�� ���� ��������
        Vector3 targetDirection = new Vector3(direction.x, 0, direction.z);

        // ������Ʈ ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.isGamePlaying && _rigid.isKinematic == false && movable)
            _rigid.velocity = new Vector3(transform.forward.x * statSO.speed, _rigid.velocity.y, transform.forward.z * statSO.speed);
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

    protected override IEnumerator Die()
    {
        GameManager.Instance.AddEarnMoney(statSO.reward);
        GameManager.Instance.AddGameScore(statSO.score);
        StartCoroutine(base.Die());
        yield return null;
    }

    protected override int GetHealth()
    {
        return statSO.health;
    }
}
