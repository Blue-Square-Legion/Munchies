using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Test_DrawBeats : MonoBehaviour
{
    public Image m_image;

    public Texture2D texture;

    public int Width = 100, Height = 100;

    public bool RunOnce = true;

    public Color Perfect = Color.blue,
        Early = Color.green,
        Late = Color.green,
        Normal = Color.cyan,
        Warn = Color.yellow,
        Miss = Color.white;

    private Color m_currentColor = Color.white;
    private int m_position = 0;
    private double m_previous;

    private Color[] m_clearColors;

    private void Start()
    {
        DrawGraph();
        enabled = false;

        m_clearColors = Enumerable.Repeat(Color.black, Height).ToArray();
        m_image.overrideSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        Invoke("StartDrawing", 0.5f);


    }

    private void OnValidate()
    {
        m_position = 0;
        DrawGraph();
        m_clearColors = Enumerable.Repeat(Color.black, Height).ToArray();
        m_image.overrideSprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    private void StartDrawing()
    {
        enabled = true;
    }

    private void Update()
    {
        if (m_previous == Conductor.Instance.data.songPosition) return;

        m_previous = Conductor.Instance.data.songPosition;

        BeatType type = BeatTracker.Instance.CheckBeat();
        switch (type)
        {
            case BeatType.Perfect: m_currentColor = Perfect; break;
            case BeatType.Early: m_currentColor = Early; break;
            case BeatType.Late: m_currentColor = Late; break;
            case BeatType.Normal: m_currentColor = Normal; break;
            case BeatType.Warn: m_currentColor = Warn; break;
            case BeatType.Miss: m_currentColor = Miss; break;
        }

        //print($"[{m_position}] {type} {Conductor.Instance.BeatFracSec}");

        DrawCurrent(m_position++);

        if (m_position >= Width)
        {
            m_position = 0;

            if (RunOnce)
            {
                enabled = false;
                texture.Apply();
            }
        }

        ClearNext(m_position);
        texture.Apply();
    }

    private void DrawCurrent(int position)
    {
        int length = 10;
        Color[] colors = Enumerable.Repeat(m_currentColor, length).ToArray();
        texture.SetPixels(position, Height / 2, 1, length, colors);
    }

    private void ClearNext(int position)
    {
        texture.SetPixels(position, 0, 1, Height, m_clearColors);
    }

    public void DrawBeat()
    {
        if (enabled == false) return;
        int length = 10;
        Color[] colors = Enumerable.Repeat(Perfect, length).ToArray();
        texture.SetPixels(m_position, Height / 2 - 10, 1, length, colors);
        texture.SetPixels(m_position, Height / 2 + 10, 1, length, colors);
    }


    public void DrawEarlyBeat()
    {
        if (enabled == false) return;
        int length = 10;
        Color[] colors = Enumerable.Repeat(Early, length).ToArray();
        texture.SetPixels(m_position, Height / 2 - 10, 1, length, colors);
    }

    public void DrawGraph()
    {
        texture = new(Width, Height, TextureFormat.RGBA32, false);
        SetFillColor(texture, Color.black);
        texture.Apply();
    }

    private void SetFillColor(Texture2D tex, Color col)
    {
        int length = tex.width * tex.height;
        tex.SetPixels(Enumerable.Repeat(col, length).ToArray());
    }
}
