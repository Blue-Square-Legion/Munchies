using System;
using UnityEngine;

[Serializable]
public struct SpawnData
{
    public GameObject prefab;
    public Vector3 position;
    public Quaternion rotation;
    public AttackData data;
    public void Spawn(AttackData data, Transform transform)
    {
        if (prefab == null) return;

        if (data.NumProjectile <= 1)
        {
            BaseAttackComponent.Spawn(prefab, data, transform.position, transform.rotation);
        }
        else
        {
            BaseAttackComponent.SpawnMultiple(prefab, data, transform.position, transform.forward);
        }
    }

    public void Spawn(Transform transform)
    {
        Spawn(data, transform);
    }

    public void Spawn()
    {
        BaseAttackComponent.Spawn(prefab, data, position, rotation);
    }
}
