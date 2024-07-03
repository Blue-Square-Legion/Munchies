using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class MusicDataFormat
{
    public string Name;
    public AudioClip clip;
    public float BPM;
}

public class PlayerMusicManager : MonoBehaviour
{
    public static PlayerMusicManager Instance;

    public Dictionary<string, MusicDataFormat> audioClips = new();

    public List<MusicDataFormat> DefaultMusic;
    public MusicDataFormat CurrentMusic;

    public static Action<MusicDataFormat> OnMusicChanged;
    public static Action<MusicDataFormat> OnMusicAdded;
    public static Action<float> OnBPMChange;

    public MusicDataEventChannel OnMusicChangeChannel;
    public MusicDataEventChannel OnMusicAddedChannel;
    public FloatEventChannel OnBPMChangeChannel;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        CurrentMusic = DefaultMusic[0];

        DefaultMusic.ForEach(data => audioClips.Add(data.clip.name, data));

        //audioClips.Add(DefaultMusic.clip.name, DefaultMusic);
        SetMusic(CurrentMusic.clip);
    }

    public MusicDataFormat Get(string name)
    {
        return audioClips[name];
    }

    public void SetMusic(string name)
    {
        SetMusic(audioClips[name].clip);
    }

    public void SetMusic(AudioClip clip)
    {
        if (!audioClips.ContainsKey(clip.name))
        {
            MusicDataFormat data = new() { clip = clip, BPM = 120 };
            audioClips.Add(clip.name, data);
            OnMusicAdded?.Invoke(data);
            OnMusicAddedChannel.Invoke(data);
        }

        CurrentMusic = audioClips[clip.name];
        OnMusicChanged?.Invoke(CurrentMusic);
        OnMusicChangeChannel.Invoke(CurrentMusic);
    }

    public void SetBPM(string bpm)
    {
        SetBPM(float.Parse(bpm));
    }

    public void SetBPM(float bpm)
    {
        CurrentMusic.BPM = bpm;
        OnBPMChange.Invoke(bpm);
        OnMusicChanged.Invoke(CurrentMusic);

        OnBPMChangeChannel.Invoke(bpm);
        OnMusicChangeChannel.Invoke(CurrentMusic);
    }
}
