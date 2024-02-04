using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PDashState : PState
{
    protected Vector2 _direction = Vector2.zero;

    protected Vector2 _initialPosition = Vector2.zero;

    public float distance;
    public float invulnerabilityTime = 0.3f;
    public float duration = 0.3f;

    public PDashState(Player player, float distance, float duration, float invulnerabilityTime) : base(player)
    {
        animatorEventName = "Dash";
        this.distance = distance;
        this.invulnerabilityTime = invulnerabilityTime;
        this.duration = duration;
    }

    public override void Enter()
    {
        base.Enter();
        _initialPosition = _player.transform.position;
        _direction = _player.moveAction.ReadValue<Vector2>();
        _player.health.InvulnerableTime = invulnerabilityTime;
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        Vector2 target = _initialPosition + _direction.normalized * distance;
        _player.moveController.MoveTowards(
            target,
            distance / _player.dashDuration
        );
        base.OnFixedUpdate();
        _timePassed += Time.fixedDeltaTime;
        if (_timePassed > duration || Vector2.Distance(_player.transform.position, target) == 0)
        {
            HandleFinish();
        }
    }
}
