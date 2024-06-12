using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CombatPlayer : BaseCombat
{
    private IA_Movement m_inputActions;

    public LayerMask GroundLayer;
    [SerializeField] private Camera m_mainCamera;

    public float forwardOffset = 2;

    protected void Awake()
    {
        m_inputActions = new();
        m_mainCamera = Camera.main;
    }

    protected void OnEnable()
    {
        m_inputActions.Player.Enable();
        m_inputActions.Player.Fire.performed -= Fire;
        m_inputActions.Player.Fire.performed += Fire;
    }

    protected void OnDisable()
    {
        m_inputActions.Player.Disable();
        m_inputActions.Player.Fire.performed -= Fire;
    }

    protected virtual void Fire(InputAction.CallbackContext obj)
    {
        if (IsMouseOverUI())
        {
            return;
        }

        TriggerAttack(transform.forward * forwardOffset);
    }

    protected bool IsMouseOverUI()
    {
        return EventSystem.current?.IsPointerOverGameObject() ?? false;
    }


    //Rotate player to mouse
    protected virtual void FixedUpdate()
    {
        Ray ray = m_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, GroundLayer))
        {
            Vector3 target = hit.point - transform.position;

            target.y = 0;

            transform.rotation = Quaternion.LookRotation(target, Vector3.up);
        }
    }

}
