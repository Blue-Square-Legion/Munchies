using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class ColorChange : MonoBehaviour
{
    [SerializeField] private float m_time = 0.1f;

    [SerializeField] private Color m_targetColor;
    [SerializeField] private AnimationCurve m_curve;

    private Image m_image;
    private Color m_defaultColor;

    private TimeoutTickPercent m_timer;

    private void Awake()
    {
        m_image = GetComponent<Image>();
        m_defaultColor = m_image.color;

        m_timer = new(m_time);
        m_timer.isRunning = false;

        m_timer.OnTick = HandleTickPercent;
    }

    private void HandleTickPercent(float percent)
    {
        m_image.color = Color.Lerp(m_defaultColor, m_targetColor, m_curve.Evaluate(percent));
    }

    public void Start()
    {
        if (m_timer.isRunning)
        {
            print($"{name} - Already Started");
            return;
        }

        m_timer.Start();
    }

    private void Update()
    {
        m_timer.Tick(Time.deltaTime);
    }
}
