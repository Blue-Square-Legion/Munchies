using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_pauseMenu;
    [SerializeField] private List<GameObject> m_playMenu;

    [SerializeField] private PauseManagerSO m_pauseManagerSO;

    private IA_Movement m_inputActions;

    private void Awake()
    {
        m_inputActions = new();
    }


    private void OnEnable()
    {
        m_inputActions.Player.Pause.Enable();
        m_inputActions.Player.Pause.performed -= TogglePause;
        m_inputActions.Player.Pause.performed += TogglePause;
    }

    private void OnDisable()
    {
        m_inputActions.Player.Pause.Disable();
        m_inputActions.Player.Pause.performed -= TogglePause;
    }

    public void TogglePause(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        m_pauseManagerSO.ToggleNotify();
        m_pauseMenu.ForEach(menu => menu.SetActive(m_pauseManagerSO.IsPaused));

        m_playMenu.ForEach(menu => menu.SetActive(!m_pauseManagerSO.IsPaused));
    }

    public void TogglePause(bool isPaused)
    {
        m_pauseManagerSO.ToggleNotify(isPaused);
        m_pauseMenu.ForEach(menu => menu.SetActive(m_pauseManagerSO.IsPaused));

        m_playMenu.ForEach(menu => menu.SetActive(!m_pauseManagerSO.IsPaused));
    }
}
