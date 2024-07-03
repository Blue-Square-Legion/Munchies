using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


[Serializable]
public class EnemySubDecider
{
    public string name;
    public BaseSensor Sensor;
    public BaseBeatAction Attack;
    public bool ShouldComplete = false;

    public virtual bool Evaluate(int frame)
    {
        return Sensor.Evaluate(frame);
    }
    public virtual Status Trigger(Enemy enemy, int frame)
    {
        return Attack.Action(enemy, frame);
    }

    public virtual void EarlyTrigger(Enemy enemy, int frame)
    {
        Attack.EarlyTrigger(enemy, frame);
    }

    public virtual void Reset()
    {
        Attack.Reset();
    }
}
