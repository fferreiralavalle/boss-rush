using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PState : State
{
    protected Player _player;
    protected string animatorEventName;

    public PState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        base.Enter();
        _player.mainAttackAction.performed += HandleMainAttack;
        _player.specialAttackAction.performed += HandleSpecial;
        _player.animator.SetBool(animatorEventName, true);
    }

    public override void Leave()
    {
        base.Leave();
        _player.mainAttackAction.performed -= HandleMainAttack;
        _player.specialAttackAction.performed -= HandleSpecial;
        _player.animator.SetBool(animatorEventName, false);
    }
    public override void OnFixedUpdate()
    {
        _player.moveController.OnFixedUpdate();
    }


    public virtual void HandleMoveStart(InputAction.CallbackContext context)
    {

    }

    public virtual void HandleMainAttack(InputAction.CallbackContext context)
    {

    }

    public virtual void HandleDashStart(InputAction.CallbackContext context)
    {

    }

    public virtual void HandleSpecial(InputAction.CallbackContext context)
    {

    }
}
