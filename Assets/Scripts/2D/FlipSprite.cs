using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFlipSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_sprite;

    [SerializeField] private bool m_isDefaultRight = true;

    public void SetRight()
    {
        m_sprite.flipX = !m_isDefaultRight;
    }

    public void SetLeft()
    {
        m_sprite.flipX = m_isDefaultRight;
    }
}
