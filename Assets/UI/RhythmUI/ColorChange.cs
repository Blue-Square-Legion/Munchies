using AnimationSO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class ColorChange : MonoBehaviour
{
    [SerializeField] private float m_time = 0.2f;

    [SerializeField] private Color m_targetColor;
    [SerializeField] private AnimationCurveSO m_curve;

    [SerializeField] private MeshRenderer m_render;
    private Color m_defaultColor;

    private TimeoutTickPercent m_timer;

    private void Awake()
    {
        if (m_render == null)
        {
            m_render = GetComponent<MeshRenderer>();
        }

        m_defaultColor = m_render.material.color;

        m_timer = new(m_time);
        m_timer.isRunning = false;

        m_timer.OnTick = HandleTickPercent;
        m_timer.OnComplete = () => m_render.material.color = m_defaultColor;
    }

    private void HandleTickPercent(float percent)
    {
        m_render.material.color = Color.Lerp(m_defaultColor, m_targetColor, m_curve.Evaluate(percent));
    }

    public void Trigger()
    {
        if (m_timer.isRunning)
        {
            return;
        }

        m_timer.Start();
    }

    private void Update()
    {
        m_timer.Tick(Time.deltaTime);
    }
}
