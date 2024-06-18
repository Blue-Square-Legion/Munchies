using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class SpawnRandomAround : Spawner
{
    [SerializeField] private float m_radius = 5;

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, m_radius);
#endif
    }


    public override GameObject Spawn()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * m_radius;

        Vector3 position = new(randomPoint.x, 0, randomPoint.y);

        return EnemyScaler.Spawn(position, transform);
    }

}
