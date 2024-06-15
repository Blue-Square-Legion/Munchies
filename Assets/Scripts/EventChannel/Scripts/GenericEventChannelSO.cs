using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace EventSO
{
    public interface IEventDataSO<T>
    {
        public void Invoke(T data);
        public void AddEventListener(Action<T> Handler);
        public void RemoveEventListener(Action<T> Handler);
    }

    public abstract class GenericEventChannelSO<T> : ScriptableObject, IEventDataSO<T>
    {
        public Action<T> OnEvent;

#if UNITY_EDITOR
        [HideInInspector] public T _testData;

        public void Test()
        {
            OnEvent?.Invoke(_testData);
        }
#endif

        public void Invoke(T data)
        {
            OnEvent?.Invoke(data);
        }

        public void AddEventListener(Action<T> Handler)
        {
            OnEvent += Handler;
        }
        public void RemoveEventListener(Action<T> Handler)
        {
            OnEvent -= Handler;
        }
    }
}

