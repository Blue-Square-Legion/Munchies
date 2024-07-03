using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedAudio : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float m_max = 1f;
    [SerializeField] private float m_min = 1.5f;


    [Header("Reference")]
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private AudioEventChannel m_eventChannel;

    private void OnEnable()
    {
        m_eventChannel.AddEventListener(PlaySound);
    }

    private void OnDisable()
    {
        m_eventChannel.RemoveEventListener(PlaySound);
    }

    private void PlaySound(AudioClip clip)
    {
        m_audioSource.pitch = Random.Range(m_min, m_max);
        m_audioSource.PlayOneShot(clip);
    }

    public void PlaySound()
    {
        m_audioSource.pitch = Random.Range(m_min, m_max);
        print(m_audioSource.pitch);
        m_audioSource.Play();
    }
}
