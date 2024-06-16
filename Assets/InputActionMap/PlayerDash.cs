using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float m_dashTime = 0.12f;
    [SerializeField] private float m_speed = 40;

    private TimeoutTick m_timeout;
    private CharacterController m_characterController;
    //private PlayerMovement m_playerMovement;
    private IA_Movement m_movement;

    private Vector3 m_direction = Vector3.zero;

    private void Awake()
    {
        m_movement = new IA_Movement();
        m_characterController = GetComponent<CharacterController>();

        m_timeout = new(m_dashTime);
        m_timeout.isRunning = false;
        m_timeout.OnTick += HandleDashTick;
        m_timeout.OnComplete += HandleDashEnd;

        //m_playerMovement = GetComponent<PlayerMovement>();
        //m_timeout.OnComplete += () => m_playerMovement.enabled = true;

    }

    private void OnEnable()
    {
        m_movement.Player.Enable();
        m_movement.Player.Sprint.performed -= OnSprintPerformed;
        m_movement.Player.Sprint.performed += OnSprintPerformed;
    }

    private void OnDisable()
    {
        m_movement.Player.Disable();
        m_movement.Player.Sprint.performed -= OnSprintPerformed;
    }

    private void Update()
    {
        m_timeout.Tick(Time.deltaTime);
    }

    private void HandleDashTick(float deltaTime)
    {
        m_characterController.Move(m_direction * deltaTime);
    }

    private void HandleDashEnd()
    {
        m_characterController.detectCollisions = true;
    }

    private void OnSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        switch (BeatTracker.Instance.CheckBeat())
        {
            case BeatType.Perfect:
            case BeatType.Normal:
                HandleDash();
                break;
        }
    }

    public void Dash(Vector3 direction)
    {
        m_direction = direction;
        m_timeout.Start();
    }

    private void HandleDash()
    {
        Vector2 val = m_movement.Player.Move.ReadValue<Vector2>() * m_speed;

        m_direction.x = val.x;
        m_direction.z = val.y;

        print($"Dash:{m_direction}");

        m_characterController.detectCollisions = false;
        m_timeout.Start();
    }
}
