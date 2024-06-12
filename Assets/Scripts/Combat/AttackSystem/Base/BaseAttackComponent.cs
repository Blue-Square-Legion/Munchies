using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class BaseAttackComponent : BaseSpawnData<AttackData>, IAttack
{
    protected new Rigidbody rigidbody;
    protected new Collider collider;

    protected override void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        base.Awake();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        OnHit(collision.gameObject);
    }

    public virtual void Attack(BaseCombat Attacker, Vector3 offset)
    {
        Spawn(gameObject, transform.position + offset, transform.rotation);
    }

    protected virtual bool TryDamage(GameObject gameObject)
    {
        if (gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(data.damage);
            return true;
        }

        return false;
    }

    #region Spawner static functions

    public static List<GameObject> SpawnMultiple(GameObject prefab, int amount, float angle, Vector3 position, Vector3 direction)
    {
        return SpawnMultiple(rotation => Spawn(prefab, position, rotation), amount, angle, direction);
    }


    public static List<GameObject> SpawnMultiple(GameObject prefab, AttackData data, Vector3 position, Vector3 direction)
    {
        return SpawnMultiple(rotation => Spawn(prefab, data, position, rotation), data.NumProjectile, data.Angle, direction);
    }

    private static List<GameObject> SpawnMultiple(Func<Quaternion, GameObject> cb, int amount, float angle, Vector3 direction)
    {
        float anglePart = angle / (amount - 1);
        float angleStart = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg - angle / 2;

        List<GameObject> list = new();

        for (var i = 0; i < amount; i++)
        {
            var rotation = Quaternion.AngleAxis(angleStart + anglePart * i, Vector3.up);
            list.Add(cb(rotation));
        }

        return list;
    }


    #endregion
}
