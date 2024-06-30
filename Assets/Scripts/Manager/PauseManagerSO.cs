using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PauseManager", menuName = "Manager/Pause")]
public class PauseManagerSO : ScriptableObject
{
    public EventSO.EventChannelSO OnPause;
    public EventSO.EventChannelSO OnResume;

    public bool IsPaused { private set; get; } = false;

    private void OnEnable()
    {
        IsPaused = false;
    }

    public bool Toggle()
    {
        if (IsPaused = !IsPaused)
        {
            PauseTime();
        }
        else
        {
            Resume();
        }

        return IsPaused;
    }

    public bool ToggleNotify()
    {
        return ToggleNotify(!IsPaused);
    }

    public bool ToggleNotify(bool isPaused)
    {
        if (IsPaused = isPaused)
        {
            NotifyPause();
        }
        else
        {
            Resume();
        }

        return IsPaused;
    }

    public void NotifyPause()
    {
        OnPause.Invoke();
        IsPaused = true;
    }

    public void PauseTime()
    {
        NotifyPause();
        Time.timeScale = 0;
        Conductor.Instance.enabled = !IsPaused;
    }

    public void Pause()
    {
        AudioListener.pause = true;
        PauseTime();
    }

    public void Resume()
    {
        OnResume.Invoke();
        AudioListener.pause = false;
        Time.timeScale = 1;
        IsPaused = false;

        Conductor.Instance.enabled = !IsPaused;
    }
}
