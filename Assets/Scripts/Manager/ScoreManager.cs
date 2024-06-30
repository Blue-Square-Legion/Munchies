using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Multiplier")]
    [SerializeField] private float m_multiplierRequiredCharge = 10;
    [SerializeField] private int m_maxMultiplier = 4;
    [SerializeField] private float m_minMultiplierCharge = 0;

    [Header("Multiplier Decay")]
    [SerializeField] private float m_multiplerDecayRatePerBeat = 0.2f;
    [SerializeField] private bool m_useMultiplerDecayRate = true;
    //[SerializeField] private bool m_decayOnSuccess = false;

    public int Multiplier = 1;
    public float MultiplierCharge = 0;

    public int Score = 0;

    [Header("Events")]
    [SerializeField] private IntEventChannel m_onScoreChange;
    [SerializeField] private IntEventChannel m_onHighScoreChange;
    [SerializeField] private IntEventChannel m_onMultiplierChange;
    [SerializeField] private FloatEventChannel m_onMultiplierChargePercentChange;

    [Header("Listeners")]
    [SerializeField] private IntEventChannel m_addScore;
    [SerializeField] private IntEventChannel m_addMutliplier;
    [SerializeField] private EventChannelSO m_resetMultiplier;
    [SerializeField] private IntEventChannel m_onBeat;

    private float m_multiplierMaxCharge;
    private readonly int m_defaultMultiplier = 1;
    public static int Highscore = 0;

    private const string HIGHSCORE = "HighScore";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        ResetMultiplier();

        m_multiplierMaxCharge = m_maxMultiplier * m_multiplierRequiredCharge - 1;

        Highscore = PlayerPrefs.GetInt(HIGHSCORE, 0);

        m_onScoreChange.Invoke(Score);
        m_onHighScoreChange.Invoke(Highscore);
    }

    private void OnEnable()
    {
        m_addMutliplier.AddEventListener(AddMultiplierCharge);
        m_addScore.AddEventListener(AddScore);
        m_resetMultiplier.AddEventListener(ResetMultiplier);

        if (m_useMultiplerDecayRate)
        {
            m_onBeat.AddEventListener(HandleDecay);
        }
    }

    private void OnDisable()
    {
        m_addMutliplier.RemoveEventListener(AddMultiplierCharge);
        m_addScore.RemoveEventListener(AddScore);
        m_resetMultiplier.RemoveEventListener(ResetMultiplier);
        m_onBeat.RemoveEventListener(HandleDecay);
    }


    public void ResetMultiplier()
    {
        Multiplier = m_defaultMultiplier;
        m_onMultiplierChange.Invoke(Multiplier);

        MultiplierCharge = m_minMultiplierCharge;
        m_onMultiplierChargePercentChange.Invoke(m_minMultiplierCharge);
    }

    public void AddMultiplierCharge(int value)
    {
        MultiplierCharge += value;

        if (MultiplierCharge > m_multiplierMaxCharge)
        {
            MultiplierCharge = m_multiplierMaxCharge;
        }

        Multiplier = m_defaultMultiplier + (int)(MultiplierCharge / m_multiplierRequiredCharge);

        NotifyChargePercent();
        m_onMultiplierChange.Invoke(Multiplier);
    }

    public void AddScore(int value)
    {
        Score += value * Multiplier;
        m_onScoreChange.Invoke(Score);

        if (Score > Highscore)
        {
            Highscore = Score;
            PlayerPrefs.SetInt(HIGHSCORE, Highscore);
            m_onHighScoreChange.Invoke(Highscore);
        }
    }
    private void HandleDecay(int frame)
    {
        if (MultiplierCharge == 0)
        {
            return;
        }

        MultiplierCharge -= m_multiplerDecayRatePerBeat;
        if (MultiplierCharge < 0)
        {
            MultiplierCharge = 0;
        }

        NotifyChargePercent();
    }


    private void NotifyChargePercent()
    {
        float chargePercent = (MultiplierCharge % m_multiplierRequiredCharge) / m_multiplierRequiredCharge;
        /*
                //TODO: fix ugly code to get percent of multiplier since above code breaks in WebGL
                //Think WebGL act odd with float % float
                float chargeModRemain = MultiplierCharge - (m_multiplierRequiredCharge * (Multiplier - 1));
                float chargePercent = chargeModRemain / m_multiplierRequiredCharge;*/
        m_onMultiplierChargePercentChange.Invoke(chargePercent);
    }


}
