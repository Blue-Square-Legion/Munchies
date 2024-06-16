using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;
using static UnityEngine.GraphicsBuffer;

public class DashAttack : BaseAttackComponent
{
    [SerializeField] private float m_speed = 40;

    public override void Attack(BaseCombat Attacker, Vector3 offset)
    {
        HandleDash();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        TryDamage(collision.gameObject);
        var dash = collision.gameObject.GetComponent<PlayerDash>();
        dash.Dash(-collision.impulse);
    }

    private void HandleDash()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");

        if (go == null)
        {
            return;
        }

        Vector3 target = go.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(target, Vector3.up);

        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * m_speed, ForceMode.Impulse);
    }
}
