using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMoveToPosition : EnemyState
{
    public Vector3 position;
    public float speedMultiplier = 1f;

    public EMoveToPosition(Enemy enemy, Vector3 position, float speedMultiplier = 1f) : base(enemy)
    {
        this.position = position;
        animatorEventName = "Move";
        this.speedMultiplier = speedMultiplier;
    }

    public override void OnFixedUpdate()
    {
        _enemy.moveController.MoveTowards(position, _enemy.moveController.speed * speedMultiplier);
        UpdateAnimatorDirection(_enemy, position - _enemy.transform.position);
        base.OnFixedUpdate();
        if (Vector3.Distance(_enemy.transform.position, position) == 0 )
        {
            HandleFinish();
        }
    }
}
