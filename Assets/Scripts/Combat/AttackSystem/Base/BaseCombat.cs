using UnityEngine;

public class BaseCombat : MonoBehaviour
{
    [SerializeField] protected BaseAttackComponent attackComponent;
    public AttackData attackData;

    public virtual void TriggerAttack(Vector3 offset)
    {
        attackComponent?.Attack(this, offset);
    }
}
