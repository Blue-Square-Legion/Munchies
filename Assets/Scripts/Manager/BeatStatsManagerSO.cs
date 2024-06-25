using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatStatsManager", menuName = "Manager/Stats")]

public class BeatStatsManagerSO : ScriptableObject
{
    public float BPM_divisor = 70;
    public float SpeedMultiplier;

    public void ChangeSpeed(float BPM)
    {
        SpeedMultiplier = BPM / BPM_divisor;
    }

    public void ChangeSpeed()
    {
        ChangeSpeed(Conductor.Instance.songBpm);
    }

}
