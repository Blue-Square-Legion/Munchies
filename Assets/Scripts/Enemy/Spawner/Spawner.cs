using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] protected EnemyScaler EnemyScaler;
    [SerializeField] protected float m_spawnTime = 1f;
    [SerializeField] protected int m_maxSpawns = 5;

    protected Interval m_cooldown;

    protected virtual void Awake()
    {
        m_cooldown = new(TrySpawn, m_spawnTime);
    }

    protected virtual void Update()
    {
        m_cooldown.Tick(Time.deltaTime);
    }


    public virtual GameObject Spawn()
    {
        return EnemyScaler.Spawn(transform.position, transform);
    }

    protected void TrySpawn()
    {
        if (transform.childCount < m_maxSpawns)
        {
            Spawn();
        }
    }
}
