using AnimationSO;
using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Util;

public class ColorChangeBeatType : MonoBehaviour
{
    [SerializeField] private Color m_perfect;
    [SerializeField] private Color m_normal;
    [SerializeField] private Color m_miss;

    [SerializeField] private IntEventChannel m_perfectEvent;
    [SerializeField] private IntEventChannel m_normalEvent;
    [SerializeField] private IntEventChannel m_missEvent;

    [SerializeField] private AnimationCurveSO m_curve;

    [SerializeField] private SpriteRenderer m_render;

    [SerializeField] private float m_displayTime = 0.3f;
    private TimeoutTickPercent m_timeout;

    private Color m_defaultColor, m_targetColor;

    private void Awake()
    {
        m_timeout = new(m_displayTime);
        m_timeout.isRunning = false;

        m_timeout.OnTick += OnTick;
        m_timeout.OnComplete = () => m_render.color = m_defaultColor;

        m_defaultColor = m_render.color;
    }

    private void Update()
    {
        m_timeout.Tick(Time.deltaTime);
    }


    private void OnEnable()
    {
        m_perfectEvent.AddEventListener(Perfect);
        m_normalEvent.AddEventListener(Normal);
        m_missEvent.AddEventListener(Miss);
    }

    private void OnDisable()
    {
        m_perfectEvent.RemoveEventListener(Perfect);
        m_normalEvent.RemoveEventListener(Normal);
        m_missEvent.RemoveEventListener(Miss);
    }


    private void OnTick(float percent)
    {
        m_render.color = Color.Lerp(m_defaultColor, m_targetColor, m_curve.Evaluate(percent));
    }

    public void Perfect(int _)
    {
        StartColor(m_perfect);
    }
    public void Normal(int _)
    {
        StartColor(m_normal);
    }
    public void Miss(int _)
    {
        StartColor(m_miss);
    }

    public void StartColor(Color color)
    {
        m_targetColor = color;
        m_timeout.Start();
    }
}
