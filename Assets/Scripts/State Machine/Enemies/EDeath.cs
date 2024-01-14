using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDeath : EnemyState
{
    public EDeath(Enemy enemy) : base(enemy)
    {
        animatorEventName = "Dead";
    }
}
