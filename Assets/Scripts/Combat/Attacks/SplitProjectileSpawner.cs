using EventSO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitProjectileSpawner : BaseProjectile
{
    public GameObject prefab;
    public List<GameObject> particleEffects;
    public bool canMultiTrigger = false;

    public Vector3 offsetPosition;

    public bool wasHit = false;

    [SerializeField] private AudioClip m_onHitNoise;
    [SerializeField] private AudioEventChannel m_playSFX;

    protected override void OnHit(GameObject target)
    {
        if (wasHit == true && !canMultiTrigger)
        {
            return;
        }

        TryDamage(target);

        foreach (var item in particleEffects)
        {
            SpawnDamageArea(item);
        }

        m_playSFX.Invoke(m_onHitNoise);
        wasHit = true;
        DestroySelf();
    }

    protected override void OnEnd()
    {
        if (prefab == null || data.SpawnLimit <= 0 || wasHit)
        {
            return;
        }

        data.SpawnLimit--;

        if (data.NumProjectile <= 1)
        {
            Spawn(prefab, data, transform.position, transform.rotation);
        }
        else
        {
            SpawnMultiple(prefab, data, transform.position, transform.forward);
        }
    }

    protected void SpawnDamageArea(GameObject target)
    {
        Spawn(target, data, transform.position + offsetPosition, Quaternion.identity);
    }
}
