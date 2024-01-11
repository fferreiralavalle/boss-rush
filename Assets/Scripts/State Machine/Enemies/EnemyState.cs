using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    protected Enemy _enemy;
    protected string animatorEventName;

    public DamageTouch damageTouch;


    public EnemyState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        _enemy.animator.SetBool(AnimatorEventName, true);
        base.Enter();
    }

    public override void Leave()
    {
        _enemy.animator.SetBool(AnimatorEventName, false);
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        _enemy.moveController.OnFixedUpdate();
        base.OnFixedUpdate();
    }

    public string AnimatorEventName { get { return animatorEventName; } set { animatorEventName = value; } }
}
