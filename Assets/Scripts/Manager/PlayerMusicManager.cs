using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class MusicDataFormat
{
    //public string Name;
    public AudioClip clip;
    public float BPM;
}

public class PlayerMusicManager : MonoBehaviour
{
    public static PlayerMusicManager Instance;

    public Dictionary<string, MusicDataFormat> audioClips = new();

    public MusicDataFormat DefaultMusic;
    public MusicDataFormat CurrentMusic;

    public static Action<MusicDataFormat> OnMusicChanged, OnMusicAdded;
    public static Action<float> OnBPMChange;

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

        CurrentMusic = DefaultMusic;


        audioClips.Add(DefaultMusic.clip.name, DefaultMusic);
        SetMusic(DefaultMusic.clip);
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
        }

        CurrentMusic = audioClips[clip.name];
        OnMusicChanged?.Invoke(CurrentMusic);
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
    }
}
