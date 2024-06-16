using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreMultiColor : MonoBehaviour
{
    [SerializeField] private List<Color> m_colors = new();

    [SerializeField] private IntEventChannel m_onMultiplierChange;

    public UnityEvent<Color> OnColorChange;

    private int m_prevMultiplier;

    private void OnEnable()
    {
        m_onMultiplierChange.AddEventListener(HandleMultiplierChange);
    }

    private void OnDisable()
    {
        m_onMultiplierChange.RemoveEventListener(HandleMultiplierChange);
    }

    private void HandleMultiplierChange(int multiplier)
    {
        if (m_prevMultiplier != multiplier)
        {
            OnColorChange.Invoke(m_colors[multiplier - 1]);
            m_prevMultiplier = multiplier;
        }
    }
}
