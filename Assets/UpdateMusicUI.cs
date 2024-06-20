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

    private void Awake()
    {
        var data = PlayerMusicManager.Instance.DefaultMusic;
        Dropdown.options.Add(new() { text = data.clip.name });
        HandleAudioChanged(data);
    }

    private void OnEnable()
    {
        PlayerMusicManager.Instance.OnBPMChange.AddListener(SetBPM);
        PlayerMusicManager.Instance.OnMusicChanged.AddListener(HandleAudioChanged);
        PlayerMusicManager.Instance.OnMusicAdded.AddListener(HandleAudioAdded);

        m_bpm.onValueChanged.AddListener(HandleBPMValueChange);
        Dropdown.onValueChanged.AddListener(HandleMusicDropDown);
    }


    private void OnDisable()
    {
        PlayerMusicManager.Instance.OnBPMChange.RemoveListener(SetBPM);
        PlayerMusicManager.Instance.OnMusicChanged.RemoveListener(HandleAudioChanged);
        PlayerMusicManager.Instance.OnMusicAdded.RemoveListener(HandleAudioAdded);

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
