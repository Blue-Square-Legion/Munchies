using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestUI : MonoBehaviour
{

    [SerializeField] private List<Color> colors = new();

    public UnityEvent<string> SongBeat;

    private void Update()
    {
        double time = AudioSettings.dspTime - Conductor.Instance.data.dspSongTime;

        SongBeat.Invoke(Conductor.Instance.data.songPosition.ToString());
    }
}
