using UnityEngine;
using UnityEngine.InputSystem;

public class PMoveState : PState
{
    public event OnMove onStop;
    public event OnMove onDash;
    public event OnMove onMainAttack;
    public event OnMove onSpecialAttack;

    public PMoveState(Player player) : base(player)
    {
        animatorEventName = "Move";
    }

    public override void Enter()
    {
        _player.moveAction.canceled += HandleMoveStart;
        _player.dashAction.performed += HandleDashStart;
        base.Enter();
    }

    public override void Leave()
    {
        _player.moveAction.canceled -= HandleMoveStart;
        _player.dashAction.performed -= HandleDashStart;
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

    public override void HandleSpecial(InputAction.CallbackContext context)
    {
        onSpecialAttack?.Invoke();
    }
}
