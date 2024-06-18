using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class EnemyScalerGroup : EnemyScaler
{
    public static int GlobalScaleCount = 0;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> m_prefab;


    private int m_randomIndex => Random.Range(0, m_prefab.Count);

    public override GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent)
    {
        return Spawn(m_prefab[m_randomIndex], GlobalScaleCount++, position, rotation, parent);
    }

    public override GameObject Spawn(Vector3 position, Transform parent)
    {
        return Spawn(m_prefab[m_randomIndex], GlobalScaleCount++, position, Quaternion.identity, parent);
    }

    public override GameObject Spawn(Vector3 position)
    {
        return Spawn(m_prefab[m_randomIndex], GlobalScaleCount++, position, Quaternion.identity, transform);
    }
}
