using UnityEngine;

public enum Status
{
    Unset,
    Running,
    Complete
}

public abstract class BaseBeatAction : MonoBehaviour
{
    public abstract Status Action(Enemy enemy, int frame);

    public abstract void Reset();
}
