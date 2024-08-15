using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbstractEnemy : MonoBehaviour
{
    public Transform hpBar;
    public Material hitMaterial;
    public MeshRenderer[] meshRenderers;
    Material[] originMats;
    protected Rigidbody _rigid;

    protected int _currentHp;
    public UnityEvent OnEnableEvent;
    public UnityEvent OnDie;

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        originMats = new Material[meshRenderers.Length];
        for (int i = 0;i < meshRenderers.Length; i++)
            originMats[i] = meshRenderers[i].material;
    }
    protected virtual void OnEnable()
    {
        _currentHp = GetHealth();
        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material = originMats[i];
        TakeDamage(0);
        OnEnableEvent?.Invoke();
    }

    public virtual void OnHitOrb(Transform player)
    {
        SoundManager.Instance.PlayAudio(Clips.OrbHit);
        TakeDamage(player.GetComponent<PlayerSkill>().copySkillStat.skillStat.circleDamage);
    }

    public void TakeDamage(int amount)
    {
        if (GameManager.Instance.isGamePlaying)
        {
            _currentHp -= amount;
            if (_currentHp <= 0)
            {
                hpBar.localScale = new Vector3(0f, 1f, 1f);
                StartCoroutine(Die());
                return;
            }
            hpBar.localScale = new Vector3((float)_currentHp / GetHealth(), 1f, 1f);
            StartCoroutine(Hit());
        }
    }
    void OnParticleCollision(GameObject other)
    {
        TakeDamage(other.transform.root.GetComponent<PlayerSkill>().copySkillStat.skillStat.attackDamage);
    }
    IEnumerator Hit()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material = hitMaterial;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material = originMats[i];
    }

    protected abstract IEnumerator Die();

    protected abstract int GetHealth();
}