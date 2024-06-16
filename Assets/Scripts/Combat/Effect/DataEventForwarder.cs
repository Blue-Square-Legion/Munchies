using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataEventForwarder : BaseSpawnData
{
    [SerializeField] private UnityEvent<BaseCombat> OnTrigger;

    public override void Trigger(BaseCombat baseCombat)
    {
        OnTrigger.Invoke(baseCombat);
    }

}
