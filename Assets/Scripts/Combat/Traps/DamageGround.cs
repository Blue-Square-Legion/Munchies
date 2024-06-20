using EventSO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class DamageGround : MonoBehaviour
{
    [SerializeField] private float m_damage = 0.5f;
    [SerializeField] private float m_tickTime = 0.1f;

    [SerializeField] private Color m_damageColor = Color.red;
    [SerializeField] private Color m_disabledColor = Color.gray;

    [SerializeField] private int m_tickCount = 2;

    [SerializeField] private IntEventChannel m_onBeat;
    [SerializeField] private SpriteRenderer m_sprite;
    [SerializeField] private bool m_isOn = true;

    private Dictionary<Collider, IDamageable> m_damageable = new();
    private Interval m_interval;


    private int m_count = 0;

    private void Awake()
    {
        m_interval = new(m_tickTime);
        m_interval.OnComplete = DamageTargets;

        ToggleDamageArea(m_isOn);
    }

    private void OnEnable()
    {
        m_onBeat.AddEventListener(HandleBeat);
    }

    private void OnDisable()
    {
        m_onBeat.RemoveEventListener(HandleBeat);
    }

    private void HandleBeat(int frame)
    {
        if (m_count++ >= m_tickCount)
        {
            ToggleDamageArea(m_isOn = !m_isOn);
            m_count = 0;
        }
    }

    private void ToggleDamageArea(bool isOn)
    {
        m_sprite.color = isOn ? m_damageColor : m_disabledColor;
    }

    private void Update()
    {
        m_interval.Tick(Time.deltaTime);
    }

    private void DamageTargets()
    {
        if (m_isOn)
        {
            foreach (IDamageable target in m_damageable.Values)
            {
                target.Damage(m_damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.TryGetComponent(out IDamageable target))
        {
            m_damageable?.Add(other, target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_damageable.Remove(other);
    }
}
