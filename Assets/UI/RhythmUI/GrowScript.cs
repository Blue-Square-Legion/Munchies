using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class GrowScript : MonoBehaviour
{


    [SerializeField] private float m_growMult = 2;
    [SerializeField] private float m_growTime = 0.1f;
    [SerializeField] private float m_shrinkTime = 0.1f;

    private RectTransform m_rectTransform;
    private TimeoutTickPercent m_growTimer, m_shrinkTimer;

    private Vector3 m_defaultScale;
    private Vector3 m_startScale;
    private Vector3 m_endScale;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_defaultScale = m_rectTransform.localScale;

        m_growTimer = new(m_growTime);
        m_growTimer.OnComplete += Shrink;
        m_growTimer.OnTick += HandleScale;
        m_growTimer.isRunning = false;

        m_shrinkTimer = new(m_shrinkTime);
        m_shrinkTimer.OnTick += HandleScale;
        m_shrinkTimer.isRunning = false;
    }

    public void Grow()
    {
        if (m_growTimer.isRunning)
        {
            print("Already Growing");
            return;
        }

        m_shrinkTimer.Stop();

        m_startScale = m_defaultScale;
        m_endScale = m_defaultScale * m_growMult;

        m_growTimer.Start();
    }

    public void Shrink()
    {
        m_startScale = m_rectTransform.localScale;
        m_endScale = m_defaultScale;

        m_shrinkTimer.Start();
    }

    private void Update()
    {
        m_growTimer.Tick(Time.deltaTime);
        m_shrinkTimer.Tick(Time.deltaTime);
    }

    private void HandleScale(float alpha)
    {
        m_rectTransform.localScale = Vector3.Lerp(m_startScale, m_endScale, alpha);
    }
}
