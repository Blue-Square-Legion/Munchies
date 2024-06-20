using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test
{
    [Serializable]
    internal struct TestBeatDrawSetting
    {
        public Color color;
        public int length;
    }

    public class Test_DrawBeats : MonoBehaviour
    {
        public UnityEngine.UI.Image m_image;

        public int Width = 100, Height = 100;

        public bool RunOnce = true;
        public bool SkipDuplicate = true;

        [SerializeField]
        private TestBeatDrawSetting m_perfect = new() { color = Color.blue, length = 10 },
            m_early = new() { color = Color.green, length = 8 },
            m_late = new() { color = Color.green, length = 8 },
            m_normal = new() { color = Color.green, length = 8 },
            m_warn = new() { color = Color.yellow, length = 6 },
            m_miss = new() { color = Color.white, length = 4 };

        private TestBeatDrawSetting m_current;
        private Texture2D m_texture;

        private double m_previous;
        private int m_position = 0;

        private Color[] m_clearColors;

        private void Start()
        {
            SetupImage();
            enabled = false;

            m_clearColors = Enumerable.Repeat(Color.black, Height).ToArray();
            m_image.overrideSprite = Sprite.Create(m_texture, new Rect(0f, 0f, m_texture.width, m_texture.height), new Vector2(0.5f, 0.5f));
            Invoke("StartDrawing", 0.5f);

            m_image.overrideSprite = Sprite.Create(m_texture, new Rect(0f, 0f, m_texture.width, m_texture.height), new Vector2(0.5f, 0.5f));
        }

        private void OnValidate()
        {
            m_position = 0;
            m_clearColors = Enumerable.Repeat(Color.black, Height).ToArray();
        }

        private void StartDrawing()
        {
            enabled = true;
        }

        private void Update()
        {
            if (SkipDuplicate && m_previous == Conductor.Instance.data.songPosition) return;

            m_previous = Conductor.Instance.data.songPosition;

            int length = 10;
            BeatType type = BeatTracker.Instance.CheckBeat();
            switch (type)
            {
                case BeatType.Perfect: m_current = m_perfect; break;
                case BeatType.Early: m_current = m_early; break;
                case BeatType.Late: m_current = m_late; break;
                case BeatType.Normal: m_current = m_normal; break;
                case BeatType.Warn: m_current = m_warn; break;
                case BeatType.Miss: m_current = m_miss; break;
            }

            DrawCurrent(m_position++, m_current);

            if (m_position >= Width)
            {
                m_position = 0;

                if (RunOnce)
                {
                    enabled = false;
                    m_texture.Apply();
                }
            }

            ClearNext(m_position);
            m_texture.Apply();
        }

        private void DrawCurrent(int position, TestBeatDrawSetting setting)
        {
            Color[] colors = Enumerable.Repeat(setting.color, setting.length).ToArray();
            m_texture.SetPixels(position, (Height - setting.length) / 2, 1, setting.length, colors);
        }

        private void ClearNext(int position)
        {
            m_texture.SetPixels(position, 0, 1, Height, m_clearColors);
        }

        public void DrawBeat()
        {
            if (enabled == false) return;
            DrawFullBar(m_perfect);
        }


        public void DrawEarlyBeat()
        {
            if (enabled == false) return;
            DrawFullBar(m_early);
        }

        private void DrawFullBar(TestBeatDrawSetting setting)
        {
            if (enabled == false) return;
            int length = setting.length;

            Color[] colors = Enumerable.Repeat(setting.color, length).ToArray();
            m_texture.SetPixels(m_position, (Height - length) / 2 - length, 1, length, colors);
            m_texture.SetPixels(m_position, (Height - length) / 2 + length, 1, length, colors);
        }

        public void SetupImage()
        {
            m_texture = new(Width, Height, TextureFormat.RGBA32, false);
            SetFillColor(m_texture, Color.black);
            m_texture.Apply();
        }

        private void SetFillColor(Texture2D tex, Color col)
        {
            int length = tex.width * tex.height;
            tex.SetPixels(Enumerable.Repeat(col, length).ToArray());
        }
    }

}

