using UnityEngine;
using Util;

public abstract class BaseProjectile : BaseAttackComponent
{
    protected Timeout lifeTimer;
    protected override void OnStart()
    {
        lifeTimer = new(DestroySelf, data.lifeTime);
        rigidbody.AddForce(transform.forward * data.speed, ForceMode.VelocityChange);
    }

    protected override void OnTick(float deltaTime)
    {
        lifeTimer.Tick(deltaTime);
    }

    protected override void OnHit(GameObject target)
    {
        TryDamage(target);
    }

    public override void Attack(BaseCombat owner, Vector3 offset)
    {
        AttackData data = owner.attackData.Clone<AttackData>();

        if (data.NumProjectile <= 1)
        {
            Spawn(gameObject, data, owner.transform.position + offset, owner.transform.rotation);
        }
        else
        {
            SpawnMultiple(gameObject, data, owner.transform.position + offset, owner.transform.forward);
        }
    }
}
