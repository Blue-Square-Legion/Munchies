using System;

[Serializable]
public struct AttackData
{
    public float damage;
    public float speed;


    public float lifeTime;
    public int SpawnLimit;

    //Multi Shot
    public int NumProjectile;
    public float Angle;

    public override string ToString()
    {
        return $"{base.ToString()} : Life {lifeTime} : Num {NumProjectile}";
    }
}
