using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AutoChase : MonoBehaviour
{
    [SerializeField] private string m_tag = "Player";

    [SerializeField] private Transform m_target;

    private Damageable m_damageable;
    private NavMeshAgent m_agent;

    private void Awake()
    {
        GameObject target = GameObject.FindGameObjectWithTag(m_tag);
        m_target = target?.transform;
        m_agent = GetComponent<NavMeshAgent>();

        m_damageable = target?.GetComponent<Damageable>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (m_damageable.isDead)
        {
            enabled = false;
            m_agent.ResetPath();
        }
        else
        {
            m_agent.SetDestination(m_target.position);
        }

    }
}
