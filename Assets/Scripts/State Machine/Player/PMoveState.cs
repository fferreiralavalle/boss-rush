using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class PMoveState : PState
{
    public event OnMove onStop;
    public event OnMove onDash;
    public event OnMove onMainAttack;

    public PMoveState(Player player) : base(player)
    {
        animatorEventName = "Move";
    }

    public override void Enter()
    {
        if (Vector2.Distance(_player.moveAction.ReadValue<Vector2>(), Vector2.zero) == 0)
        {
            onStop?.Invoke();
            return;
        }
        _player.moveAction.canceled += HandleMoveStart;
        _player.dashAction.performed += HandleDashStart;
        _player.mainAttackAction.performed += HandleMainAttack;
        base.Enter();
    }

    public override void Leave()
    {
        _player.moveAction.canceled -= HandleMoveStart;
        _player.dashAction.performed -= HandleDashStart;
        _player.mainAttackAction.performed -= HandleMainAttack;
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        Vector2 movement = _player.moveAction.ReadValue<Vector2>();
        _player.moveController.MoveTowards((Vector2)_player.transform.position + movement);
        base.OnFixedUpdate();
        UpdateAnimatorDirection(_player, movement);
    }

    public override void HandleMoveStart(InputAction.CallbackContext context)
    {
        onStop?.Invoke();
        Debug.Log("Move Cancelled");
    }

    public override void HandleDashStart(InputAction.CallbackContext context)
    {
        onDash?.Invoke();
    }

    public override void HandleMainAttack(InputAction.CallbackContext context)
    {
        onMainAttack?.Invoke();
    }
}
