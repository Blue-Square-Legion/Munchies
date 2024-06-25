using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Util;


[Serializable]
public struct BeatLeniency
{
    public float before;
    public float after;
}


/*
 * TODO: Clean up beat system.
 * Rather than child CombatPlayer
 * Should move Beat System to seperate system * 
 */
public class BeatCombatPlayer : CombatPlayer
{
    [SerializeField] private AttackData m_missAttack, m_perfectAttack;
    [SerializeField] private BaseAttackComponent m_critAttack;

    [Header("Attack CD")]
    [SerializeField, Range(0.01f, 1), Tooltip("Attack Cooldown = percent of Beat in seconds")]
    private float m_attackCooldownPercent = 0.8f;
    [SerializeField] private float m_minAttackCooldown = 0.2f;

    private Timeout m_timeout;
    private bool m_canShoot = true;

    public UnityEvent m_frameAlreadyCompleted;

    private AttackData m_normalAttack;


    public void HandleBPMChange()
    {
        float cooldownTime = Mathf.Max((float)Conductor.Instance.data.secPerBeat * m_attackCooldownPercent, m_minAttackCooldown);
        m_timeout.targetTime = cooldownTime;
    }


    private void Start()
    {
        m_normalAttack = attackData;

        m_timeout = new(m_minAttackCooldown);
        m_timeout.isRunning = false;
        m_timeout.OnStart = () => m_canShoot = false;
        m_timeout.OnComplete = () => m_canShoot = true;

        HandleBPMChange();
    }

    private void Update()
    {
        m_timeout.Tick(Time.deltaTime);
    }

    protected override void Fire(InputAction.CallbackContext obj)
    {
        if (IsShootingDisabled)
        {
            return;
        }

        if (!m_canShoot)
        {
            m_frameAlreadyCompleted.Invoke();
            return;
        }

        switch (BeatTracker.Instance.CheckBeat())
        {
            case BeatType.Warn:
                TriggerMissAttack();
                return;
            case BeatType.Perfect:
                TriggerCritAttack();
                break;
            case BeatType.Early:
                TriggerNormalAttack();
                break;
            case BeatType.Late:
                TriggerNormalAttack();
                break;
            case BeatType.Miss:
                TriggerMissAttack();
                break;
        }
    }

    private void TriggerNormalAttack()
    {
        attackData = m_normalAttack;
        TriggerAttack(transform.forward * forwardOffset);
        m_timeout.Start();
    }

    private void TriggerCritAttack()
    {
        attackData = m_perfectAttack;
        m_critAttack?.Attack(this, transform.forward * forwardOffset);
        m_timeout.Start();
    }

    private void TriggerMissAttack()
    {
        attackData = m_missAttack;
        TriggerAttack(transform.forward * forwardOffset);
        m_timeout.Start();
    }

}