using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Direction2D : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_agent;

    public bool isRight = false;
    public UnityEvent OnRightDirection, OnLeftDirection;


    private void Update()
    {
        if (m_agent == null)
        {
            return;
        }

        if (isRight && m_agent.velocity.x < 0)
        {
            isRight = false;
            OnLeftDirection.Invoke();
        }
        else if (!isRight && m_agent.velocity.x > 0)
        {
            isRight = true;
            OnRightDirection.Invoke();
        }

    }
}
