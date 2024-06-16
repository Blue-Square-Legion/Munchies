using UnityEngine;

public class DashAttack : BaseAttackComponent
{
    [SerializeField] private string m_tag = "Player";
    private Transform m_target;

    protected override void Init()
    {
        m_target = GameObject.FindGameObjectWithTag(m_tag).transform;
    }

    public override void Attack(BaseCombat Attacker, Vector3 offset)
    {
        HandleDash();
    }

    //overrid to access collision data. Maybe should provide to OnHit?
    protected override void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(m_tag))
        {
            return;
        }

        TryDamage(collision.gameObject);

        var dash = collision.gameObject.GetComponent<PlayerDash>();
        dash.Dash(-collision.impulse);  //Add fake knockback
    }

    private void HandleDash()
    {
        if (m_target == null)
        {
            return;
        }

        //Rotate to target
        Vector3 target = m_target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(target, Vector3.up);

        //Dash to target
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * data.speed, ForceMode.Impulse);
    }
}
