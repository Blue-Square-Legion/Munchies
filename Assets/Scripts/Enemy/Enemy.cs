using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseCombat
{
    [SerializeField] private IntEventChannel m_onBeat;
    [SerializeField] private List<EnemySubDecider> m_enemyDeciders = new();

    public NavMeshAgent Agent;
    public Rigidbody Rigidbody;
    public Collider Collider;
    public Damageable Damageable;
    private EnemySubDecider m_currentDecider;
    private bool m_needsNew = true;

    private void Awake()
    {
        if (Agent == null)
        {
            Agent = GetComponent<NavMeshAgent>();
        }

        if (Rigidbody == null)
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        if (Collider == null)
        {
            Collider = GetComponent<Collider>();
        }

        if (Damageable == null)
        {
            Damageable = GetComponent<Damageable>();
        }
    }


    private void OnEnable()
    {
        m_onBeat.AddEventListener(Evaluate);
    }

    private void OnDisable()
    {
        m_onBeat.RemoveEventListener(Evaluate);
    }



    public void Evaluate(int frame)
    {
        if (m_needsNew || !EvaluateCurrent(frame))
        {
            //Get new Action
            m_currentDecider?.Reset();
            m_currentDecider = m_enemyDeciders.Find(decider => decider.Evaluate(frame));

            if (m_currentDecider == null)
            {
                DefaultLogic();
                return;
            }
        }

        m_needsNew = false;
        switch (m_currentDecider.Trigger(this, frame))
        {
            case Status.Complete:
                m_needsNew = true;
                break;
            case Status.Unset:  //On Cooldown
                m_needsNew = true;
                break;
        }
    }

    private void DefaultLogic()
    {
        //TODO
    }

    private bool EvaluateCurrent(int frame)
    {
        return m_currentDecider.ShouldComplete || m_currentDecider.Evaluate(frame);
    }
}
