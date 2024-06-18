using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private string m_refName = "MasterVol";
    [SerializeField] private float m_defaultVolume = 0.8f;

    private void Awake()
    {
        float value = PlayerPrefs.GetFloat(m_refName, m_defaultVolume);
        SetVolume(value);

        Slider slider = GetComponentInChildren<Slider>();
        slider.value = value;
    }

    public void SetVolume(float value)
    {
        m_audioMixer.SetFloat(m_refName, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(m_refName, value);
    }
}
