using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Setting")]
    [SerializeField] private float m_speed = 5f;

    [Header("Jump Setting")]
    [SerializeField] private bool m_enableJump = true;
    [SerializeField] private float m_maxJumpHeight = 4f;
    [SerializeField] private float m_maxJumpTime = 0.75f;

    [SerializeField] private BeatStatsManagerSO m_statsManagerSO;

    private IA_Movement m_inputActions;
    private CharacterController m_controller;

    private Vector3 m_movement = Vector3.zero;
    private float m_groundGravity = -0.05f;
    private float m_initialJumpVelocity, m_gravity;

    private float m_fallMulti = 2;
    private bool m_isJumpPressed = false;
    private int m_jumpCount = 0, m_maxAirJump = 2;

    private void SetupJumpVariables()
    {
        float timeToApex = m_maxJumpTime / 2;
        m_gravity = (-2 * m_maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        m_initialJumpVelocity = 2 * m_maxJumpHeight / timeToApex;
    }

    private void Awake()
    {
        m_inputActions = new IA_Movement();
        m_controller = GetComponent<CharacterController>();

        SetupJumpVariables();
    }

    private void OnEnable()
    {
        m_inputActions.Player.Enable();
        m_inputActions.Player.Move.performed += OnMove;
        m_inputActions.Player.Move.canceled += OnMove;

        m_inputActions.Player.Jump.started += OnJumpStart;
        m_inputActions.Player.Jump.canceled += OnJumpEnd;
    }

    private void OnDisable()
    {
        m_inputActions.Player.Disable();
        m_inputActions.Player.Move.performed -= OnMove;
        m_inputActions.Player.Move.canceled -= OnMove;

        m_inputActions.Player.Jump.started -= OnJumpStart;
        m_inputActions.Player.Jump.canceled -= OnJumpEnd;
    }

    private void FixedUpdate()
    {
        m_controller.Move(m_movement * Time.fixedDeltaTime);
        HandleGravity();
        HandleJump();
    }

    private void HandleGravity()
    {
        bool isFalling = m_movement.y <= 0 || !m_isJumpPressed;

        if (m_controller.isGrounded)
        {
            m_movement.y = m_groundGravity;
        }
        else if (isFalling)
        {
            m_movement.y = Mathf.Max(AverageVelocity(m_movement.y, m_gravity * Time.deltaTime * m_fallMulti), -20f);
        }
        else
        {
            m_movement.y = AverageVelocity(m_movement.y, m_gravity * Time.deltaTime);
        }
    }

    private float AverageVelocity(float current, float gravity)
    {
        return (current + (current + gravity)) * 0.5f;
    }

    private void OnJumpStart(InputAction.CallbackContext context)
    {
        m_isJumpPressed = true;
        if (m_enableJump && (m_controller.isGrounded || m_jumpCount < m_maxAirJump))
        {
            m_movement.y = m_initialJumpVelocity * 0.5f;
        }
    }
    private void OnJumpEnd(InputAction.CallbackContext context)
    {
        m_isJumpPressed = false;
        m_jumpCount++;
    }

    private void HandleJump()
    {
        if (m_controller.isGrounded && !m_isJumpPressed)
        {
            m_jumpCount = 0;
        }
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 val = context.ReadValue<Vector2>() * m_speed * m_statsManagerSO.SpeedMultiplier;

        m_movement.x = val.x;
        m_movement.z = val.y;
    }
}
