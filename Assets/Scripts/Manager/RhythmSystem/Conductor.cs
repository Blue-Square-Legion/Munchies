using EventSO;
using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public struct ConductorData
{
    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;
}

public class Conductor : MonoBehaviour
{
    public static Conductor Instance;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //Trigger on beat abit early to allow for animation timing
    public float preBeatTime = 0.1f;

    public ConductorData data;

    public UnityEvent<int> OnBeatBefore;
    public UnityEvent<int> OnBeatCurrent;

    public int songPositionInBeatInt { get; private set; }
    public float BeatFrac => data.songPositionInBeats - (int)data.songPositionInBeats;
    public float songPositionInBeats => data.songPositionInBeats;

    private AudioSource m_musicSource;
    private int m_beatTracker = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Load the AudioSource attached to the Conductor GameObject
        m_musicSource = GetComponent<AudioSource>();

        data = new() { secPerBeat = 60 / songBpm };
    }

    private void Start()
    {
        //Record the time when the music starts
        data.dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        m_musicSource.Play();
    }



    // Update is called once per frame
    private void Update()
    {
        //determine how many seconds since the song started
        data.songPosition = (float)(AudioSettings.dspTime - data.dspSongTime);

        //determine how many beats since the song started
        data.songPositionInBeats = data.songPosition / data.secPerBeat;

        int songPosition = (int)data.songPositionInBeats;

        if ((int)(data.songPositionInBeats + preBeatTime) != m_beatTracker)
        {
            //Trigger Early beat for animations
            m_beatTracker = songPosition + 1;
            OnBeatBefore.Invoke(m_beatTracker);
        }
        else if (songPosition != songPositionInBeatInt)
        {
            //Trigger On Beat timing for Enemy
            songPositionInBeatInt = songPosition;
            OnBeatCurrent.Invoke(songPositionInBeatInt);
        }
    }
}
