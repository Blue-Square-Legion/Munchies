using AnimationSO;
using EventSO;
using UnityEngine;
using Util;

public class GrowScript : MonoBehaviour
{
    [SerializeField] private float m_time = 0.2f;

    [SerializeField] private Vector3 m_targetScale = Vector3.one;
    [SerializeField] private AnimationCurveSO m_curve;

    [SerializeField] private IntEventChannel m_onBeforeBeat;

    private TimeoutTickPercent m_timer;
    private Vector3 m_defaultScale;

    private void Awake()
    {
        m_timer = new(m_time);
        m_timer.isRunning = false;
        m_timer.OnTick = HandleTickPercent;

        m_defaultScale = transform.localScale;
    }

    private void OnEnable()
    {
        m_onBeforeBeat.AddEventListener(StartEffect);
    }

    private void OnDisable()
    {
        m_onBeforeBeat.RemoveEventListener(StartEffect);
    }

    private void HandleTickPercent(float percent)
    {
        transform.localScale = Vector3.Lerp(m_defaultScale, m_targetScale, m_curve.Evaluate(percent));
    }

    private void Update()
    {
        m_timer.Tick(Time.deltaTime);
    }

    public void StartEffect(int _)
    {
        if (m_timer.isRunning)
        {
            Debug.LogWarning($"{name} Grow - Already Started");
            return;
        }

        m_timer.Start();
    }

}
