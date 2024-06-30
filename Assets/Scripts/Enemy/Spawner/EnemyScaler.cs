using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[Serializable]
public struct ScaleData<T>
{
    public T MinValue; public T MaxValue;

    public int StartScale;
    public int EndScale;

    public bool Enable;
}


public abstract class EnemyScaler : MonoBehaviour
{
    private static readonly int m_defaultScale = 10;

    [SerializeField] private BeatStatsManagerSO m_manager;

    [Header("NavMesh Settings")]
    [SerializeField] private ScaleData<float> m_speed = new() { Enable = true, MinValue = 3.5f, MaxValue = 10f, EndScale = m_defaultScale };
    [SerializeField] private ScaleData<float> m_angularSpeed = new() { Enable = true, MinValue = 120, MaxValue = 360, EndScale = m_defaultScale };
    [SerializeField] private ScaleData<float> m_acceleration = new() { Enable = true, MinValue = 8, MaxValue = 12, EndScale = m_defaultScale };

    [Header("Stats Setting")]
    [SerializeField] private ScaleData<float> m_health = new() { Enable = true, MinValue = 2, MaxValue = 10f, EndScale = m_defaultScale };
    [SerializeField] private ScaleData<float> m_damage = new() { Enable = true, MinValue = 1, MaxValue = 3, EndScale = m_defaultScale };

    public virtual GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        return Spawn(prefab, 0, position, rotation, parent);
    }


    public abstract GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent);
    public abstract GameObject Spawn(Vector3 position, Transform parent);
    public abstract GameObject Spawn(Vector3 position);

    public GameObject Spawn(GameObject prefab, int spawnCount, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go = Instantiate(prefab, position, rotation, parent);

        var enemy = go.GetComponent<Enemy>();

        if (m_speed.Enable)
            enemy.Agent.speed = CalcSpeed(m_speed, spawnCount);

        if (m_angularSpeed.Enable)
            enemy.Agent.angularSpeed = CalcSpeed(m_angularSpeed, spawnCount);

        if (m_acceleration.Enable)
            enemy.Agent.acceleration = CalcSpeed(m_acceleration, spawnCount);

        if (m_health.Enable)
            enemy.Damageable.SetMaxHealth(Calc(m_health, spawnCount));

        if (m_damage.Enable)
            enemy.attackData.damage = Calc(m_damage, spawnCount);

        return go;
    }

    protected float Calc(ScaleData<float> data, int spawnCount)
    {
        float percent = (spawnCount - data.StartScale) / (data.EndScale - data.StartScale);

        if (percent < 1)
        {
            return Mathf.Lerp(data.MinValue, data.MaxValue, percent);
        }
        return data.MaxValue;
    }

    protected float CalcSpeed(ScaleData<float> data, int spawnCount)
    {
        float percent = (spawnCount - data.StartScale) / (data.EndScale - data.StartScale);

        if (percent < 1)
        {
            return Mathf.Lerp(data.MinValue, data.MaxValue, percent) * m_manager.SpeedMultiplier;
        }
        return data.MaxValue;
    }
}
