using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnReachTarget(Transform _target);
public delegate void OnFailToReachTarget();
public class EChaseState : EnemyState
{
    public event OnReachTarget onReachTarget;
    public event OnFailToReachTarget onFailToReachTarget;

    public float timeToReachBeforeGiveup = 5f;

    protected Transform _target;
    protected float _wantedDistance;

    public EChaseState(Enemy enemy, float wantedDistance, DamageTouch damageTouch) : base(enemy)
    {
        _wantedDistance = wantedDistance;
        animatorEventName = "Chase";
        this.damageTouch = damageTouch;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (Target == null)
        {
            onFailToReachTarget?.Invoke();
            return;
        }
        _enemy.moveController.MoveTowards(_target.position);
        float distance = Vector2.Distance(_target.position, _enemy.transform.position);
        if (distance < _wantedDistance)
        {
            onReachTarget?.Invoke(_target);
        }
        _timePassed += Time.fixedDeltaTime;
        if (_timePassed < timeToReachBeforeGiveup)
        {
            onFailToReachTarget?.Invoke();
        }
    }

    public Transform Target { get { return _target; } set { _target = value; } }
}
