using UnityEngine;

public abstract class BaseSpawnData : MonoBehaviour
{
    [SerializeField] protected AttackData data;

    protected virtual void Awake()
    {
        Init();
    }

    protected virtual void OnEnable()
    {
        //Only called in scene. Not called via Instantiate
        OnStart();
    }

    protected virtual void FixedUpdate()
    {
        OnTick(Time.deltaTime);
    }

    public virtual void Trigger(BaseCombat baseCombat) { }
    public virtual void CleanUp() { OnEnd(); }
    protected virtual void DestroySelf() { OnEnd(); Destroy(gameObject); }
    protected virtual void Init() { }
    protected virtual void OnHit(GameObject target) { }
    protected virtual void OnStart() { }
    protected virtual void OnEnd() { }
    protected virtual void OnTick(float deltaTime) { }
    public static bool TryDamage(GameObject gameObject, float damage)
    {
        if (gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(damage);
            return true;
        }

        return false;
    }
    public static GameObject Spawn(GameObject target, AttackData data, Vector3 position, Quaternion rotation)
    {
        GameObject go = Spawn(target, out BaseSpawnData component, position, rotation);

        if (component) { component.data = data; component?.OnStart(); }


        return go;
    }

    public static GameObject Spawn(GameObject target, Vector3 position, Quaternion rotation)
    {
        GameObject go = Spawn(target, out BaseSpawnData component, position, rotation);
        component?.OnStart();
        return go;
    }

    public static GameObject Spawn<ComponentType>(GameObject target, out ComponentType component, Vector3 position, Quaternion rotation) where ComponentType : Component
    {
        GameObject go = Instantiate(target, position, rotation);
        go.TryGetComponent<ComponentType>(out component);
        return go;
    }
}
