using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float m_dashTime = 0.12f;
    [SerializeField] private float m_speed = 40;

    [SerializeField] private float m_perfectMulti = 1.5f;
    [SerializeField] private float m_normalMulti = 1f;
    [SerializeField] private float m_missMulti = 0.5f;


    [Header("Dash CD")]
    [SerializeField, Range(0.01f, 1), Tooltip("Attack Cooldown = percent of Beat in seconds")]
    private float m_CooldownPercent = 0.8f;
    [SerializeField] private float m_minCooldown = 0.2f;

    private Timeout m_cooldownTimeout;
    private bool m_onCooldown = false;

    private TimeoutTick m_timeout;
    private CharacterController m_characterController;
    private Collider m_collider;
    //private PlayerMovement m_playerMovement;
    private IA_Movement m_movement;

    private Vector3 m_direction = Vector3.zero;

    private void Awake()
    {
        m_movement = new IA_Movement();
        m_characterController = GetComponent<CharacterController>();
        m_collider = GetComponent<Collider>();
    }


    private void Start()
    {
        m_timeout = new(m_dashTime);
        m_timeout.isRunning = false;
        m_timeout.OnTick += HandleDashTick;
        m_timeout.OnComplete += HandleDashEnd;

        //m_playerMovement = GetComponent<PlayerMovement>();
        //m_timeout.OnComplete += () => m_playerMovement.enabled = true;


        m_cooldownTimeout = new(m_minCooldown);
        m_cooldownTimeout.isRunning = false;
        m_cooldownTimeout.OnStart = () => m_onCooldown = false;
        m_cooldownTimeout.OnComplete = () => m_onCooldown = true;

        HandleBPMChange();
    }

    public void HandleBPMChange()
    {
        float cooldownTime = Mathf.Max((float)Conductor.Instance.data.secPerBeat * m_CooldownPercent, m_minCooldown);
        m_cooldownTimeout.targetTime = cooldownTime;
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



    private void OnSprintPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (m_onCooldown) return;

        BeatType type = BeatTracker.Instance.CheckBeat();
        switch (type)
        {
            case BeatType.Perfect:
                HandleDash(m_perfectMulti);
                break;
            case BeatType.Normal:
            case BeatType.Late:
            case BeatType.Early:
                HandleDash(m_normalMulti);
                break;
            default:
                HandleDash(m_missMulti);
                break;
        }
    }

    public void Dash(Vector3 direction)
    {
        m_direction = direction;
        m_timeout.Start();
    }

    private void HandleDash(float multiplier = 1)
    {
        Vector2 val = m_movement.Player.Move.ReadValue<Vector2>().normalized * m_speed * multiplier;

        m_direction.x = val.x;
        m_direction.z = val.y;

        m_characterController.detectCollisions = false;
        m_timeout.Start();
    }

    private void HandleDashEnd()
    {
        m_characterController.detectCollisions = true;
    }
}
