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
    public BeatLeniency m_beatLeniency = new() { before = 0.1f, after = 0.1f };
    public BeatLeniency m_beatPerfect = new() { before = 0.01f, after = 0.01f };

    public UnityEvent OnPerfect, OnEarly, OnLate, OnFailedTime;
    public UnityEvent<int> OnFailedFrame;


    [SerializeField] private int m_failedFrame;

    private void Start()
    {
        m_beatLeniency.before /= Conductor.Instance.secPerBeat;
        m_beatLeniency.after /= Conductor.Instance.secPerBeat;

        m_beatPerfect.before /= Conductor.Instance.secPerBeat;
        m_beatPerfect.after /= Conductor.Instance.secPerBeat;
    }

    protected override void Fire(InputAction.CallbackContext obj)
    {
        if (IsMouseOverUI())
        {
            return;
        }

        if (m_failedFrame == Conductor.Instance.songPositionInBeatInt)
        {
            return;
        }

        if (IsWithinTolerance(m_beatPerfect))
        {
            OnPerfect.Invoke();
            TriggerAttack(transform.forward * forwardOffset);
        }
        else if (IsWithinToleranceNotify(m_beatLeniency))
        {
            TriggerAttack(transform.forward * forwardOffset);
        }
        else
        {
            //fail next beat
            m_failedFrame = Conductor.Instance.songPositionInBeatInt + 1;
            OnFailedFrame.Invoke(m_failedFrame);
        }

    }

    private bool IsWithinTolerance(BeatLeniency beatLeniency)
    {
        float position = Conductor.Instance.songPositionInBeats - Conductor.Instance.songPositionInBeatInt;

        if (position < beatLeniency.after)
        {
            return true;
        }

        if (1 - position < beatLeniency.before)
        {
            return true;
        }

        return false;
    }

    private bool IsWithinToleranceNotify(BeatLeniency beatLeniency)
    {
        float position = Conductor.Instance.songPositionInBeats - Conductor.Instance.songPositionInBeatInt;

        if (position < beatLeniency.after)
        {
            OnLate.Invoke();
            return true;
        }

        if (1 - position < beatLeniency.before)
        {
            OnEarly.Invoke();
            return true;
        }

        OnFailedTime.Invoke();
        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnPerfect.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            OnEarly.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            OnLate.Invoke();
        }
    }
}