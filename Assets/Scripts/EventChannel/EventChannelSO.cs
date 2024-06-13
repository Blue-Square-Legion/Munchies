using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventSO
{
    interface IEventSO
    {
        public void Invoke();
        public void AddEventListener(Action Handler);
        public void RemoveEventListener(Action Handler);
    }

    [CreateAssetMenu(fileName ="Event", menuName ="Event/Void")]
    public class EventChannelSO : ScriptableObject, IEventSO
    {
        public Action EventChannel;

        public void AddEventListener(Action Handler)
        {
            EventChannel += Handler;
        }

        public void Invoke()
        {
            EventChannel?.Invoke();
        }

        public void RemoveEventListener(Action Handler)
        {
            EventChannel -= Handler;
        }
    }

}

