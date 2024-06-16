using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : BaseCombat
{
    [SerializeField] private IntEventChannel m_onBeat;
    [SerializeField] private List<EnemySubDecider> m_enemyDeciders = new();

    private EnemySubDecider m_currentDecider;

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
        if (m_currentDecider == null || (!m_currentDecider.ShouldComplete && !m_currentDecider.Evaluate(frame)))
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

        switch (m_currentDecider.Trigger(this, frame))
        {
            case Status.Complete:
                m_currentDecider = null;
                break;
            case Status.Unset:  //On Cooldown
                m_currentDecider = null;
                break;
        }
    }

    private void DefaultLogic()
    {
        //TODO
    }
}
