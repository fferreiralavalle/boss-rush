using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EIdleState : EnemyState
{
    public float idleTime = 1f;

    public EIdleState(Enemy enemy, float idleTime) : base(enemy)
    {
        this.idleTime = idleTime;
        animatorEventName = "Idle";
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void OnUpdate()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed > idleTime)
        {
            HandleFinish();
        }
    }
}
