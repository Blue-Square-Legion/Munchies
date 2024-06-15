using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EventSO
{
    public abstract class GenericEventListener<T> : MonoBehaviour
    {
        [SerializeField] protected GenericEventChannelSO<T> m_eventChannel;
        public UnityEvent<T> OnEvent;

        protected virtual void OnEnable()
        {
            m_eventChannel.AddEventListener(TriggerEvent);
        }

        protected virtual void OnDisable()
        {
            m_eventChannel.RemoveEventListener(TriggerEvent);
        }

        public void TriggerEvent(T value)
        {
            OnEvent?.Invoke(value);
        }
    }
}

