using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Util;
using static UnityEngine.UI.GridLayoutGroup;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private string m_tag = "Player";
    [SerializeField] private float m_damage = 1;

    private void OnCollisionEnter(Collision collision)
    {
        HandleDamage(collision.collider);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleDamage(collision.collider);
    }

    /*    private void OnTriggerEnter(Collider other)
        {
            HandleDamage(other);
        }
        private void OnTriggerStay(Collider other)
        {
            HandleDamage(other);
        }
    */
    private void HandleDamage(Collider other)
    {
        if (CheckTarget(other, out IDamageable target))
        {
            target.Damage(m_damage);
        }
    }

    private bool CheckTarget(Collider other, out IDamageable target)
    {
        target = null;
        return IsTarget(other) && other.TryGetComponent<IDamageable>(out target);
    }

    private bool IsTarget(Collider other)
    {
        return other.CompareTag(m_tag);
    }
}
