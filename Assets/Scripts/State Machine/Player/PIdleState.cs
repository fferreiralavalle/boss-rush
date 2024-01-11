using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public delegate void OnMove();

public class PIdleState : PState
{
    public event OnMove onMove;
    public event OnMove onMainAttack;
    public PIdleState(Player player) : base(player)
    {
        animatorEventName = "Idle";
    }

    public override void Enter()
    {
        base.Enter();
        _player.moveAction.performed += HandleMoveStart;
        _player.mainAttackAction.performed += HandleMainAttack;
    }

    public override void Leave()
    {
        base.Leave();
        _player.moveAction.performed -= HandleMoveStart;
        _player.mainAttackAction.performed -= HandleMainAttack;
    }

    public override void HandleMoveStart(InputAction.CallbackContext context)
    {
        onMove?.Invoke();
    }

    public override void HandleMainAttack(InputAction.CallbackContext context)
    {
        onMainAttack?.Invoke();
    }
}
