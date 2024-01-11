using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class DamageSummary
{
    public float damage = 0;
    public DamageType damageType;
    public Vector2 force = Vector2.zero;

    public DamageSummary(float damage, DamageType damageType = DamageType.Normal)
    {
        this.damage = damage;
        this.damageType = damageType;
    }


}
