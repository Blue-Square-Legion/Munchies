using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Util;

public class JumpBeatAction : BaseSpawnData
{
    public float start = 0, end = 20;
    public float time = 0.3f;
    public Transform target;

    public LayerMask mask;
    public float radius = 2f;

    public UnityEvent OnJump, OnLand;

    private TimeoutTickPercent m_timer;

    [SerializeField] private bool m_isJumping = false;
    private Enemy m_enemy;


    protected override void Init()
    {
        m_timer = new(time);
        m_timer.isRunning = false;
        m_timer.OnTick = HandleTickPercent;
        m_timer.OnComplete = Swap;
    }

    private void Swap()
    {
        var temp = start;
        start = end;
        end = temp;

        if (!m_isJumping)
        {
            m_enemy.Collider.enabled = true;
            OnLand.Invoke();
            Collider[] hits = Physics.OverlapSphere(target.position, radius, mask);

            foreach (var item in hits)
            {
                TryDamage(item.gameObject, data.damage);
            }
        }
    }


    private void HandleTickPercent(float percent)
    {
        var pos = target.localPosition;
        pos.y = Mathf.Lerp(start, end, percent);
        target.localPosition = pos;
    }

    protected override void OnTick(float deltaTime)
    {
        m_timer.Tick(deltaTime);
    }

    public override void Trigger(BaseCombat baseCombat)
    {
        m_isJumping = !m_isJumping;
        m_timer.Start();

        if (m_isJumping)
        {
            m_enemy = (Enemy)baseCombat;
            m_enemy.Collider.enabled = false;
            OnJump.Invoke();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, radius);
    }
}
