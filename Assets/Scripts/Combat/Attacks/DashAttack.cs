using UnityEngine;
using UnityEngine.AI;

public class DashAttack : BaseAttackComponent
{
    [SerializeField] private string m_tag = "Player";
    private Transform m_target;
    private bool m_hit = false;

    protected override void Init()
    {
        m_target = GameObject.FindGameObjectWithTag(m_tag).transform;
    }

    public override void Attack(BaseCombat Attacker, Vector3 offset)
    {
        m_hit = false;
        HandleDash();
    }

    //overrid to access collision data. Maybe should provide to OnHit?
    protected override void OnCollisionEnter(Collision collision)
    {
        if (m_hit || !collision.gameObject.CompareTag(m_tag))
        {
            return;
        }

        m_hit = TryDamage(collision.gameObject);

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
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * data.speed, ForceMode.Impulse);
    }

    protected override void OnEnd()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.isKinematic = true;
    }
}
