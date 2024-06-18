using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataEventForwarder : BaseSpawnData
{
    [SerializeField] private UnityEvent<BaseCombat> m_onTrigger;

    public override void Trigger(BaseCombat baseCombat)
    {
        m_onTrigger.Invoke(baseCombat);
    }

}
