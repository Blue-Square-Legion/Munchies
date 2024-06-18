using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericEventListener : MonoBehaviour
{
    [SerializeField] private EventChannelSO m_eventChannel;
    public UnityEvent OnEvent;

    private void OnEnable()
    {
        m_eventChannel.AddEventListener(TriggerEvent);
    }

    private void OnDisable()
    {
        m_eventChannel.RemoveEventListener(TriggerEvent);
    }

    public void TriggerEvent()
    {
        OnEvent?.Invoke();
    }
}
