using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private EventSO.EventChannelSO m_onScore;
    [SerializeField] private TMPro.TMP_Text m_text;

    private int m_score = 0;

    private void OnEnable()
    {
        m_onScore.AddEventListener(HandleScore);
    }

    private void OnDisable()
    {
        m_onScore.RemoveEventListener(HandleScore);
    }

    private void Start()
    {
        UpdateScore(0);
    }

    private void HandleScore()
    {
        UpdateScore(++m_score);
    }

    private void UpdateScore(int score)
    {
        m_text.text = $"Score: {score}";
    }
}
