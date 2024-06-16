using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


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
    [SerializeField] private int m_completedFrame;

    public UnityEvent m_frameAlreadyCompleted;

    private float m_baseDamage;

    private void Start()
    {
        m_baseDamage = attackData.damage;
    }

    protected override void Fire(InputAction.CallbackContext obj)
    {
        if (IsShootingDisabled)
        {
            return;
        }

        if (m_completedFrame == Conductor.Instance.songPositionInBeatInt)
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
            case BeatType.Normal:
                TriggerNormalAttack();
                break;
            case BeatType.Miss:
                m_completedFrame = Conductor.Instance.songPositionInBeatInt + 1;
                break;
        }

        m_completedFrame = Conductor.Instance.songPositionInBeatInt + 1;
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