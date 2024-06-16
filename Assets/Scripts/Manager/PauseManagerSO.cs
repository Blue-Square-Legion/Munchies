using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PauseManager", menuName = "Manager/Pause")]
public class PauseManagerSO : ScriptableObject
{
    public bool IsPaused { private set; get; } = false;

    private void OnEnable()
    {
        IsPaused = false;
    }

    public void Toggle()
    {
        if (IsPaused = !IsPaused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        AudioListener.pause = true;
        Time.timeScale = 0;
        IsPaused = true;
    }

    public void Resume()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
        IsPaused = false;
    }
}
