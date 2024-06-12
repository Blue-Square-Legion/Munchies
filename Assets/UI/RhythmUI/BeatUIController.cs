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

    private float m_start, m_end;
    public Vector3 position = Vector3.zero;

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");

        BeatCombatPlayer combat = go.GetComponent<BeatCombatPlayer>();

        combat.OnFailedFrame.AddListener(HandleFailedFrame);

        //delay for Race condition for parent width setting.
        Invoke("SetUpWidth", 0.1f);
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
        if (beatOfThisNote == beatFrame)
        {
            m_image.color = Color.red;
        }
    }

    private void Update()
    {
        float alpha = (BeatsShownInAdvance - (beatOfThisNote - Conductor.Instance.songPositionInBeats)) / BeatsShownInAdvance;
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
        beatOfThisNote += BeatsShownInAdvance;
        m_image.color = Color.white;
    }
}
