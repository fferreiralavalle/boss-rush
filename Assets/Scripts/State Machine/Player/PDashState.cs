using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PDashState : PState
{
    protected Vector2 _direction = Vector2.zero;

    protected Vector2 _initialPosition = Vector2.zero;

    public PDashState(Player player) : base(player)
    {
        animatorEventName = "Dash";
    }

    public override void Enter()
    {
        base.Enter();
        _initialPosition = _player.transform.position;
        _direction = _player.moveAction.ReadValue<Vector2>();
        _player.health.InvulnerableTime = _player.dashInvulnerability;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        Vector2 target = _initialPosition + _direction.normalized * _player.dashDistance;
        _player.moveController.MoveTowards(
            target,
            _player.moveController.Speed / _player.dashDuration
        );
        base.OnFixedUpdate();
        _timePassed += Time.fixedDeltaTime;
        if (_timePassed > _player.dashDuration || Vector2.Distance(_player.transform.position, target) == 0)
        {
            HandleFinish();
        }
    }
}
