using System;
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
    public UnityEvent<int> OnPerfect;
    public UnityEvent<int> OnNormal;
    //public UnityEvent OnEarly, OnLate;

    [Tooltip("Warn Player miss, but not failure")]
    public UnityEvent<int> OnWarn;

    [Tooltip("On Player Failure")]
    public UnityEvent<int> OnMissFrame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void TriggerBeat()
    {
        CheckBeat();
    }

    public BeatType CheckBeat()
    {
        double position = Conductor.Instance.BeatFracSec;
        int frame = Conductor.Instance.songPositionInBeatInt;

        //Perfect Handler
        switch (IsWithinTolerance(m_beatPerfect, position))
        {
            case BeatType.Early:
                OnPerfect.Invoke(frame + 1);
                return BeatType.Perfect;
            case BeatType.Late:
                OnPerfect.Invoke(frame);
                return BeatType.Perfect;
        }

        //Normal Handler
        switch (IsWithinTolerance(m_beatNormal, position))
        {
            case BeatType.Early:
                OnNormal.Invoke(frame + 1);
                return BeatType.Early;
            case BeatType.Late:
                OnNormal.Invoke(frame);
                return BeatType.Late;
        }

        //Handle Warn & Miss
        if (IsWithinTolerance(m_beatWarn, position) == BeatType.Miss)
        {
            OnMissFrame.Invoke(frame + 1);
            return BeatType.Miss;
        }
        else
        {
            OnWarn.Invoke(frame);
            return BeatType.Warn;
        }
    }


    private BeatType IsWithinTolerance(BeatLeniency beatLeniency, double position)
    {
        if (position < beatLeniency.after)
        {
            return BeatType.Late;
        }

        if (Conductor.Instance.data.secPerBeat - position < beatLeniency.before)
        {
            return BeatType.Early;
        }

        return BeatType.Miss;
    }

}
