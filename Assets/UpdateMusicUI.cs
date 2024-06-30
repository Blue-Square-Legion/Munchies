using System;
using System.Collections;
using System.Collections.Generic;
using Test;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMusicUI : MonoBehaviour
{
    public TMP_Dropdown Dropdown;

    [SerializeField] private TMP_Text m_musicName;
    [SerializeField] private TMP_InputField m_bpm;
    [SerializeField] private Test_DrawBeats m_drawBeats;

    public List<BeatUIController> m_beatList = new();

    private void Start()
    {
        var data = PlayerMusicManager.Instance.DefaultMusic;

        data.ForEach(music =>
        {
            Dropdown.options.Add(new() { text = music.clip.name });
        });

        HandleAudioChanged(PlayerMusicManager.Instance.CurrentMusic);

        int index = Dropdown.options.FindIndex(data => data.text == PlayerMusicManager.Instance.CurrentMusic.clip.name);

        print(index);

        Dropdown.SetValueWithoutNotify(index);
        Dropdown.RefreshShownValue();
    }

    private void OnEnable()
    {
        PlayerMusicManager.OnBPMChange += SetBPM;
        PlayerMusicManager.OnMusicChanged += HandleAudioChanged;
        PlayerMusicManager.OnMusicAdded += HandleAudioAdded;

        m_bpm.onValueChanged.AddListener(HandleBPMValueChange);
        Dropdown.onValueChanged.AddListener(HandleMusicDropDown);
    }


    private void OnDisable()
    {
        PlayerMusicManager.OnBPMChange += SetBPM;
        PlayerMusicManager.OnMusicChanged += HandleAudioChanged;
        PlayerMusicManager.OnMusicAdded += HandleAudioAdded;

        m_bpm.onValueChanged.RemoveListener(HandleBPMValueChange);
        Dropdown.onValueChanged.RemoveListener(HandleMusicDropDown);
    }

    public void HandleBPMValueChange(string bpm)
    {
        PlayerMusicManager.Instance.SetBPM(bpm);
    }

    public void HandleAudioAdded(MusicDataFormat data)
    {
        Dropdown.options.Add(new() { text = data.clip.name });
        Dropdown.SetValueWithoutNotify(Dropdown.options.Count - 1);
    }

    public void HandleAudioChanged(MusicDataFormat data)
    {
        m_musicName.text = data.clip.name;
        SetBPM(data.BPM);

        foreach (var item in m_beatList)
        {
            item.Reset();
        }
    }

    public void SetBPM(string bpm)
    {
        SetBPM(float.Parse(bpm));
    }


    public void SetBPM(float bpm)
    {
        m_bpm.SetTextWithoutNotify(bpm.ToString());
    }


    private void HandleMusicDropDown(int index)
    {
        string name = Dropdown.options[index].text;
        var data = PlayerMusicManager.Instance.Get(name);

        HandleAudioChanged(data);
        PlayerMusicManager.Instance.SetMusic(name);
    }

}
