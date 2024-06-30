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

    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private Renderer m_render;
    [SerializeField] private string m_materialKey = "_GlowColor";

    [SerializeField] private float m_displayTime = 0.3f;
    private TimeoutTickPercent m_timeout;

    private Color m_defaultSpriteColor, m_defaultRenderColor, m_targetColor;

    private void Awake()
    {
        m_timeout = new(m_displayTime);
        m_timeout.isRunning = false;

        m_timeout.OnTick += OnTick;
        m_timeout.OnComplete = () =>
        {
            m_spriteRenderer.color = m_defaultRenderColor;
            m_render.materials[1].SetColor(m_materialKey, m_defaultRenderColor);
        };

        m_defaultRenderColor = Color.black;
        m_defaultSpriteColor = m_spriteRenderer.color;
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
        float val = m_curve.Evaluate(percent);
        m_spriteRenderer.color = Color.Lerp(m_defaultSpriteColor, m_targetColor, val / 2);

        Color GlowColor = Color.Lerp(m_defaultRenderColor, m_targetColor, val);
        m_render.materials[1].SetColor(m_materialKey, GlowColor);
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
