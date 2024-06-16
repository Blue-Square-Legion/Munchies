using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerSensor : BaseSensor
{
    [SerializeField] private Collider m_collider;
    [SerializeField] private string m_tag = "Player";

    private bool m_isTargetInRange = false;

    private void Awake()
    {
        if (m_collider == null)
        {
            m_collider = GetComponent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            m_isTargetInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            m_isTargetInRange = false;
        }
    }

    public override bool Evaluate(int frame)
    {
        return m_isTargetInRange;
    }
}