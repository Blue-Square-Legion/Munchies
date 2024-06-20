using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class BeatUIController : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private RectTransform m_rectTransform;

    public float start = 0f;
    public float end = 500f;
    public bool flip = false;


    public int BeatsShownInAdvance = 4;
    public int beatOfThisNote = 4;

    private int m_beatOfThisNote = 4;

    private float m_start, m_end;
    public Vector3 position = Vector3.zero;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");

        BeatTracker.Instance.OnMissFrame.AddListener(HandleFailedFrame);

        m_beatOfThisNote = beatOfThisNote;

        //delay for Race condition for parent width setting.
        Invoke("SetUpWidth", 0.1f);
    }

    public void Reset()
    {
        m_beatOfThisNote = beatOfThisNote;
    }

    private void SetUpWidth()
    {
        end = transform.parent.GetComponent<RectTransform>().rect.width;
        m_end = end;

        if (!flip)
        {
            m_start = start;
            m_end = end;
        }
        else
        {
            m_start = end;
            m_end = start;
        }

    }

    private void HandleFailedFrame(int beatFrame)
    {
        if (m_beatOfThisNote == beatFrame)
        {
            m_image.color = Color.red;
        }
    }

    private void Update()
    {
        float alpha = (float)((BeatsShownInAdvance - (m_beatOfThisNote - Conductor.Instance.songPositionInBeats)) / BeatsShownInAdvance);
        if (alpha <= 1)
        {
            position.x = Mathf.Lerp(m_start, m_end, alpha);
        }
        else
        {
            ResetUI();
        }

        transform.localPosition = position;
        //m_rectTransform.localPosition = position;
    }

    private void ResetUI()
    {
        m_beatOfThisNote += BeatsShownInAdvance;
        m_image.color = Color.white;
    }
}
