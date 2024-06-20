using System;
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
    [SerializeField] private BaseAttackComponent m_critAttack;

    private Timeout m_timeout;
    private bool m_canShoot = true;

    public UnityEvent m_frameAlreadyCompleted;

    private float m_baseDamage;

    private void Start()
    {
        m_baseDamage = attackData.damage;

        float cooldownTime = Mathf.Max((float)Conductor.Instance.data.secPerBeat * 0.25f, 0.2f);

        print(cooldownTime);

        m_timeout = new(cooldownTime);
        m_timeout.isRunning = false;
        m_timeout.OnStart = () => m_canShoot = false;
        m_timeout.OnComplete = () => m_canShoot = true;
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
                break;
        }


    }

    private void TriggerNormalAttack()
    {
        attackData.damage = m_baseDamage;
        TriggerAttack(transform.forward * forwardOffset);
    }

    private void TriggerCritAttack()
    {
        attackData.damage = 2 * m_baseDamage;
        m_critAttack?.Attack(this, transform.forward * forwardOffset);
    }

}