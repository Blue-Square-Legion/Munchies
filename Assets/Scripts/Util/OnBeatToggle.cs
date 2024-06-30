using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class OnBeatToggle : MonoBehaviour
    {

        [SerializeField] private IntEventChannel m_onBeat;

        [SerializeField] private bool m_isActive = false;

        public UnityEvent OnActiveBeat;
        public UnityEvent OnOffBeat;

        private void OnEnable()
        {
            m_onBeat.AddEventListener(HandleBeat);
        }

        private void OnDisable()
        {
            m_onBeat.RemoveEventListener(HandleBeat);
        }

        private void HandleBeat(int obj)
        {
            m_isActive = !m_isActive;
            if (m_isActive)
            {
                OnActiveBeat.Invoke();
            }
            else
            {
                OnOffBeat.Invoke();
            }
        }
    }

}
