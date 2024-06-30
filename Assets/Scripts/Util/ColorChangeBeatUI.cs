using AnimationSO;
using EventSO;
using System;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class ColorChangeBeatUI : MonoBehaviour
{
    [Header("Color Setting")]
    [SerializeField] private Color m_perfect = Color.yellow;
    [SerializeField] private Color m_normal = Color.green;
    [SerializeField] private Color m_miss = Color.gray;

    [SerializeField, Range(0, 1)] private float m_colorMulti = 0.5f;

    [SerializeField] private AnimationCurveSO m_curve;
    [SerializeField] private float m_displayTime = 0.3f;

    [Header("Event Channels")]

    [SerializeField] private IntEventChannel m_perfectEvent;
    [SerializeField] private IntEventChannel m_normalEvent;
    [SerializeField] private IntEventChannel m_missEvent;

    [Header("Renderer")]
    [SerializeField] private Image m_render;


    private TimeoutTickPercent m_timeout;

    private Color m_defaultRenderColor, m_targetColor;

    private void Awake()
    {
        m_timeout = new(m_displayTime);
        m_timeout.isRunning = false;

        m_timeout.OnTick += OnTick;
        m_timeout.OnComplete = () =>
        {
            m_render.color = m_defaultRenderColor;
        };

        m_defaultRenderColor = m_render.color;
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
        m_render.color = Color.Lerp(m_defaultRenderColor, m_targetColor, val) * m_colorMulti;
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
