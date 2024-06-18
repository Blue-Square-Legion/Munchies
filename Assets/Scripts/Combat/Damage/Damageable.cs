using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour, IDamageable
{
    [SerializeField] private float m_maxHP = 10;

    [Header("Short Damage invuln CD")]
    [SerializeField] private bool m_hasDamageInvuln = true;
    [SerializeField] private float m_damageInvulnCDTime = 0.1f;

    [Header("Regen")]
    [SerializeField] private bool m_hasRegen = true;
    [SerializeField, Tooltip("How much healed per Regen tick")] private float m_regenAmount = 0.5f;
    [SerializeField, Tooltip("Regen tick time in Sec")] private float m_regenTime = 1f;

    [Header("Death")]
    [SerializeField] private bool m_destroySelf = false;
    [SerializeField] private float m_destroyTime = 2;


    [Header("Events")]
    public UnityEvent<float> OnHPChangePercent;
    public UnityEvent<float> OnDamaged, OnHeal;
    public UnityEvent OnDeath;

    public float HP { get; protected set; }

    [Header("Stats")]
    public bool isDamagable = true;
    public bool isDead = false;


    private void Awake()
    {
        Init();
    }

    private IEnumerator RegenHealth()
    {
        while (m_hasRegen && !isDead)
        {
            if (isDamagable)
            {
                Heal(m_regenAmount);
            }

            yield return new WaitForSeconds(m_regenTime);
        }
    }

    private void StartInvuln()
    {
        if (m_hasDamageInvuln)
        {
            StartCoroutine(InvulnTimer());
        }
    }

    private IEnumerator InvulnTimer()
    {
        isDamagable = false;
        yield return new WaitForSeconds(m_damageInvulnCDTime);
        isDamagable = true;
    }

    public void SetMaxHealth(float health)
    {
        m_maxHP = health;
        HP = health;
    }

    public void Damage(float damage)
    {
        if (!isDamagable || isDead)
        {
            return;
        }

        HP = Mathf.Max(HP - damage, 0);

        OnHPChangePercent.Invoke(HP / m_maxHP);

        if (HP <= 0)
        {
            OnDeath.Invoke();
            isDead = true;

            if (m_destroySelf) { Destroy(gameObject, m_destroyTime); }
        }
        else
        {
            StartInvuln();
            OnDamaged.Invoke(damage);
        }
    }

    public void Heal(float heal)
    {
        if (isDead)
        {
            return;
        }

        if (HP >= m_maxHP)
        {
            return;
        }

        HP = Mathf.Min(HP + heal, m_maxHP);

        OnHeal.Invoke(heal);
        OnHPChangePercent.Invoke(HP / m_maxHP);
    }

    public void Init()
    {
        HP = m_maxHP;
        isDamagable = true;
        isDead = false;
        OnHPChangePercent.Invoke(HP / m_maxHP);
        if (m_hasRegen)
        {
            StartCoroutine(RegenHealth());
        }
    }
}
