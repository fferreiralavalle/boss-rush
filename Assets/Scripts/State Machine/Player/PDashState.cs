using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PDashState : PState
{
    protected Vector2 _direction = Vector2.zero;

    public PDashState(Player player) : base(player)
    {
        animatorEventName = "Dash";
    }

    public override void Enter()
    {
        base.Enter();
        _direction = _player.moveAction.ReadValue<Vector2>();
        _player.health.InvulnerableTime = _player.dashInvulnerability;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        _player.moveController.MoveTowards(
            (Vector2)_player.transform.position + _direction.normalized * _player.dashDistance,
            _player.dashDistance / _player.dashDuration
        );
        base.OnFixedUpdate();
        _timePassed += Time.fixedDeltaTime;
        if (_timePassed > _player.dashDuration)
        {
            HandleFinish();
        }
    }
}
