using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class DamageSummary: ICloneable
{
    public float damage = 0;
    public DamageType damageType;
    public Vector3 force = Vector3.zero;

    public DamageSummary(float damage, DamageType damageType = DamageType.Normal)
    {
        this.damage = damage;
        this.damageType = damageType;
    }

    public object Clone()
    {
        return MemberwiseClone();
    }
}
