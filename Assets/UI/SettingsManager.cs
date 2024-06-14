using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer m_audioMixer;

    public void SetMasterVolume(float sliderValue)
    {
        m_audioMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
    }
}
