using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRandomAround : Spawner
{

    [SerializeField] private float m_radius = 5;


    [Header("Scaling Settings")]
    [SerializeField] private int m_maxCount = 10;
    [SerializeField] private float m_minSpeed = 3.5f, m_maxSpeed = 10;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, m_radius);
#endif
    }


    public override GameObject Spawn()
    {
        m_spawnCount++;
        Vector2 randomPoint = Random.insideUnitCircle.normalized * m_radius;

        Vector3 position = new(randomPoint.x, 0, randomPoint.y);

        GameObject go = Instantiate(prefab, position, Quaternion.identity, transform);

        NavMeshAgent agent = go.GetComponent<NavMeshAgent>();

        if (m_spawnCount < m_maxCount)
        {
            agent.speed = Mathf.Lerp(m_minSpeed, m_maxSpeed, m_spawnCount / m_maxCount);
        }
        else
        {
            agent.speed = m_maxSpeed;
        }

        return go;
    }

}
