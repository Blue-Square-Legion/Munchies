using System;

[Serializable]
public class AttackData : ICloneable
{
    public float damage;
    public float speed;

    public float lifeTime;
    public int SpawnLimit;

    //Multi Shot
    public int NumProjectile;
    public float Angle;

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public T Clone<T>() where T : AttackData
    {
        return (T)this.MemberwiseClone();
    }

    public override string ToString()
    {
        return $"{base.ToString()} : Life {lifeTime} : Num {NumProjectile}";
    }
}
