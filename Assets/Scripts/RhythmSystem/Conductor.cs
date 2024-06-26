using EventSO;
using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public struct ConductorData
{
    //The number of seconds for each song beat
    public double secPerBeat;

    //CurrentMusic song position, in seconds
    public double songPosition;

    //CurrentMusic song position, in beats
    public double songPositionInBeats;

    //How many seconds have passed since the song started
    public double dspSongTime;
}

public class Conductor : MonoBehaviour
{
    [SerializeField] private MusicDataEventChannel m_onMusicChanged;

    public static Conductor Instance;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //Trigger on beat abit early to allow for animation timing
    public float preBeatTime = 0.1f;

    public ConductorData data;

    public UnityEvent<int> OnBeatBefore;
    public UnityEvent<int> OnBeatCurrent;
    public UnityEvent<float> OnBPMChange;

    public int songPositionInBeatInt { get; private set; }
    public double BeatFrac => data.songPositionInBeats - (int)data.songPositionInBeats;
    public double songPositionInBeats => data.songPositionInBeats;

    private AudioSource m_musicSource;
    private int m_beforeBeatTracker = 0;

    public double BeatFracSec { get; private set; }

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

        OnBPMChange.Invoke(songBpm);
    }

    private void OnEnable()
    {
        m_onMusicChanged.AddEventListener(HandleMusicChange);
        //PlayerMusicManager.OnMusicChanged += HandleMusicChange;
    }

    private void OnDisable()
    {
        m_onMusicChanged.RemoveEventListener(HandleMusicChange);
        //PlayerMusicManager.OnMusicChanged -= HandleMusicChange;
    }

    private void HandleMusicChange(MusicDataFormat data)
    {
        print($"Music Changed: {data.Name}");

        songBpm = data.BPM;
        Setup();
        m_musicSource.clip = data.clip;
        m_musicSource.Play();

        OnBPMChange.Invoke(songBpm);
    }

    private void Start()
    {
        HandleMusicChange(PlayerMusicManager.Instance.CurrentMusic);
    }

    private void Setup()
    {
        data = new() { secPerBeat = 60 / songBpm, dspSongTime = AudioSettings.dspTime };
    }

    // Update is called once per frame
    private void Update()
    {
        //determine how many seconds since the song started
        data.songPosition = (float)(AudioSettings.dspTime - data.dspSongTime);

        //determine how many beats since the song started
        data.songPositionInBeats = data.songPosition / data.secPerBeat;

        int songPosition = (int)data.songPositionInBeats;

        //determine how time difference after beat
        BeatFracSec = data.songPosition - (data.secPerBeat * songPositionInBeatInt);

        if ((int)(data.songPositionInBeats + preBeatTime) != m_beforeBeatTracker)
        {
            //Trigger Early beat for animations
            m_beforeBeatTracker = songPosition + 1;
            OnBeatBefore.Invoke(m_beforeBeatTracker);
        }
        else if (songPosition != songPositionInBeatInt)
        {
            //Trigger On Beat timing for Enemy
            songPositionInBeatInt = songPosition;
            OnBeatCurrent.Invoke(songPositionInBeatInt);
        }


    }
}
