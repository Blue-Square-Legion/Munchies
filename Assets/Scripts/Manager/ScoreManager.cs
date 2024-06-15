using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManagerOld : MonoBehaviour
{

    [Header("Multiplier")]
    [SerializeField] private float m_multiplierRequiredCharge = 25;
    [SerializeField] private int m_maxMultiplier = 4;

    [SerializeField] private float m_multiplerDecayRatePerBeat = 0.2f;
    [SerializeField] private bool m_useMultiplerDecayRate = true;
    [SerializeField] private bool m_decayOnSuccess = false;

    public int Multiplier = 1;
    public int MultiplierCharge = 0;

    public int Score = 0;


    [SerializeField] private IntEventChannel m_addMutliplier, m_addScore;
    [SerializeField] private EventChannelSO m_resetMultiplier;

    public void ResetMultiplier()
    {
        Multiplier = 1;
    }
}
