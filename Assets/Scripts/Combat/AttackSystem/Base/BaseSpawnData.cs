using UnityEngine;

public abstract class BaseSpawnData<T> : MonoBehaviour
{
    [SerializeField] protected T data;

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

    protected virtual void DestroySelf() { OnEnd(); Destroy(gameObject); }
    protected virtual void Init() { }
    protected virtual void OnHit(GameObject target) { }
    protected virtual void OnStart() { }
    protected virtual void OnEnd() { }
    protected virtual void OnTick(float deltaTime) { }

    public static GameObject Spawn(GameObject target, T data, Vector3 position, Quaternion rotation)
    {
        GameObject go = Spawn(target, out BaseSpawnData<T> component, position, rotation);

        if (component) { component.data = data; component?.OnStart(); }


        return go;
    }

    public static GameObject Spawn(GameObject target, Vector3 position, Quaternion rotation)
    {
        GameObject go = Spawn(target, out BaseSpawnData<T> component, position, rotation);
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
