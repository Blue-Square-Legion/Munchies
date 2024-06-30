using AnimationSO;
using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

public class DisplayTimingResult : MonoBehaviour
{
    public TMP_Text Text;

    [SerializeField] private float m_time = 0.2f;
    [SerializeField] private AnimationCurveSO m_curve;

    [SerializeField] private IntEventChannel m_perfectEvent, m_normalEvent, m_missEvent;

    [SerializeField]
    private Color m_perfect = Color.yellow,
        m_normal = Color.green,
        m_miss = Color.gray;

    private TimeoutTickPercent m_timer;
    private Color m_default;
    private Color m_targetColor;

    private void Awake()
    {
        m_timer = new(m_time);
        m_timer.isRunning = false;
        m_timer.OnTick = HandleTickPercent;

        m_default = Text.color;
    }

    private void OnEnable()
    {
        m_perfectEvent.AddEventListener(Perfect);
        m_normalEvent.AddEventListener(Normal);
        m_missEvent.AddEventListener(Miss);
    }

    private void Miss(int obj)
    {
        StartEffect("Miss");
    }

    private void Normal(int obj)
    {
        StartEffect("Normal");
    }

    private void Perfect(int obj)
    {
        StartEffect("Perfect");
    }

    private void HandleTickPercent(float percent)
    {
        Text.color = Color.Lerp(m_default, m_targetColor, m_curve.Evaluate(percent));
    }

    private void Update()
    {
        m_timer.Tick(Time.deltaTime);
    }

    public void StartEffect(string type)
    {
        Text.text = type;

        switch (type)
        {
            case "Perfect": m_targetColor = m_perfect; break;
            case "Normal": m_targetColor = m_normal; break;
            case "Miss": m_targetColor = m_miss; break;
            default: m_targetColor = Color.white; break;
        }
        m_timer.Start();
    }
}
