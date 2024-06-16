using UnityEngine;

public abstract class BaseSensor : MonoBehaviour
{
    public abstract bool Evaluate(int frame);
}
