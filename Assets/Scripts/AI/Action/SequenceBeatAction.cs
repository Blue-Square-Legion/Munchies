using System.Collections.Generic;
using UnityEngine;


public class SequenceBeatAction : BaseBeatAction
{
    [SerializeField] private List<BaseSpawnData> m_list;
    [SerializeField] private int m_cooldown = 2;

    private int m_endFrame = 0, m_lastFrame = 0, m_count = 0;
    private Status m_status = Status.Unset;
    private BaseSpawnData m_current;

    public override Status Action(Enemy enemy, int frame)
    {
        if (IsOnCooldown(frame)) { return Status.Unset; }

        if (m_status != Status.Running || frame - m_lastFrame != 1)
        {
            StartUp(frame);
        }

        m_lastFrame = frame;

        m_current?.CleanUp();
        m_current = m_list[m_count++];
        m_current?.Trigger(enemy);

        if (m_count >= m_list.Count)
        {
            EndSequence(frame);
            return m_status = Status.Complete;
        }

        return m_status = Status.Running;
    }
    public override void Reset()
    {
        m_current?.CleanUp();
        m_current = null;
        m_status = Status.Unset;
    }

    private void StartUp(int frame)
    {
        m_status = Status.Running;
        m_count = 0;
        m_current = null;
    }

    private void EndSequence(int frame)
    {
        m_endFrame = frame;
    }

    private bool IsOnCooldown(int frame)
    {
        return frame - m_endFrame <= m_cooldown;
    }
}