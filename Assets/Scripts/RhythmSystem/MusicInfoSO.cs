using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/Data")]
public class MusicInfoSO : ScriptableObject
{
    public AudioClip AudioClip;
    public float BPM;
}
