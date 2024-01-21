using UnityEngine.InputSystem;

public delegate void OnMove();

public class PIdleState : PState
{
    public event OnMove onMove;
    public event OnMove onMainAttack;
    public event OnMove onSpecialAttack;
    public PIdleState(Player player) : base(player)
    {
        animatorEventName = "Idle";
    }

    public override void Enter()
    {
        base.Enter();
        _player.moveAction.performed += HandleMoveStart;
    }

    public override void Leave()
    {
        base.Leave();
        _player.moveAction.performed -= HandleMoveStart;
    }

    public override void HandleMoveStart(InputAction.CallbackContext context)
    {
        onMove?.Invoke();
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
