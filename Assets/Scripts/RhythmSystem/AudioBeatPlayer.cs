using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBeatPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource m_source;

    private Dictionary<AudioClip, AudioSource> m_audioPlayerList = new();

    private void Awake()
    {
        if (m_source == null) m_source = GetComponent<AudioSource>();
    }

    public void PlayOnBeat(int frame)
    {
        int position = Conductor.Instance.songPositionInBeatInt;

        double time = Conductor.Instance.data.songPosition - position * Conductor.Instance.data.secPerBeat;
        print($"pos: {position} frame:{frame} time:{Conductor.Instance.data.secPerBeat - time}");
        m_source.PlayScheduled(AudioSettings.dspTime + time);
    }

}
