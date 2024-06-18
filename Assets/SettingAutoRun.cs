using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAutoRun : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        VolumeControl[] settings = GetComponentsInChildren<VolumeControl>(true);

        print(settings.Length);
        foreach (var item in settings)
        {
            item.Init();
        }
    }
}
