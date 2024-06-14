using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BeatType
{
    Perfect,
    Normal,
    Early,
    Late,
    Warn,
    Miss
}

public class BeatTracker : MonoBehaviour
{
    public static BeatTracker Instance;

    [Header("Tolerance settings")]
    [SerializeField] private BeatLeniency m_beatNormal = new() { before = 0.1f, after = 0.1f };
    [SerializeField] private BeatLeniency m_beatPerfect = new() { before = 0.01f, after = 0.01f };
    [SerializeField] private BeatLeniency m_beatWarn = new() { before = 0, after = 0.3f };


    // Perfect > Normal > Warn > Miss
    [Header("Beat Events")]
    public UnityEvent OnPerfect;
    public UnityEvent OnNormal;
    //public UnityEvent OnEarly, OnLate;

    [Tooltip("Warn Player miss, but not failure")]
    public UnityEvent OnWarn;

    [Tooltip("On Player Failure")]
    public UnityEvent<int> OnMissFrame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        float BeatPerSec = Conductor.Instance.songBpm / 60f;

        m_beatNormal.before *= BeatPerSec;
        m_beatNormal.after *= BeatPerSec;

        m_beatPerfect.before *= BeatPerSec;
        m_beatPerfect.after *= BeatPerSec;

        m_beatWarn.before *= BeatPerSec;
        m_beatWarn.after *= BeatPerSec;
    }

    public BeatType CheckBeat()
    {
        float position = Conductor.Instance.songPositionInBeats - Conductor.Instance.songPositionInBeatInt;

        if (IsWithinTolerance(m_beatPerfect, position) != BeatType.Miss)
        {
            OnPerfect.Invoke();
            return BeatType.Perfect;
        }

        switch (IsWithinTolerance(m_beatNormal, position))
        {
            case BeatType.Late:
                OnNormal.Invoke();
                //OnLate.Invoke(); 
                return BeatType.Normal;
            case BeatType.Early:
                OnNormal.Invoke();
                //OnEarly.Invoke();
                return BeatType.Normal;
            default:
                return HandleMiss(position);
        }
    }

    private BeatType HandleMiss(float position)
    {
        if (IsWithinTolerance(m_beatWarn, position) == BeatType.Miss)
        {
            int failedFrame = Conductor.Instance.songPositionInBeatInt + 1;
            OnMissFrame.Invoke(failedFrame);
            return BeatType.Miss;
        }
        else
        {
            OnWarn.Invoke();
            return BeatType.Warn;
        }
    }


    private BeatType IsWithinTolerance(BeatLeniency beatLeniency, float position)
    {
        if (position < beatLeniency.after)
        {
            return BeatType.Late;
        }

        if (1 - position < beatLeniency.before)
        {
            return BeatType.Early;
        }

        return BeatType.Miss;
    }

}
