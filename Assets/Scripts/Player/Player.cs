using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void OnProjectileSpawn(Projectile projectile);

public class Player : Entity
{
    public float baseHealth = 10f;
    public PlayerWeapons playerWeapons;
    public PlayerInputs playerInput;

    public float invulnerabilityAfterHit = 0.5f;
    public float dashInvulnerability = 0.7f;
    public float dashDistance = 2f;
    public float dashDuration = 1f;

    public InputAction moveAction;
    public InputAction dashAction;
    public InputAction mainAttackAction;

    protected PIdleState _idleState;
    protected PMoveState _moveState;
    protected PDashState _dashState;
    protected PMainAttack _mainAttackState;

    private void Awake()
    {
        playerInput = new PlayerInputs();
    }

    private void OnEnable()
    {
        moveAction = playerInput.Player.Move;
        moveAction.Enable();
        dashAction = playerInput.Player.Dash;
        dashAction.Enable();
        mainAttackAction = playerInput.Player.Fire;
        mainAttackAction.Enable();

        health.onDamage += (DamageSummary damage, float curr) => UIPlayerInfo.Instance.UpdateHealth();
        health.onDamage += ActivateGloryTime;

        PowerManager.Instance.onPowerObtain += HandlePowerChange;

        InitializeStates();
    }

    public void HandlePowerChange(Power change)
    {
        health.maxHealth = baseHealth - PowerManager.Instance.GetPowersTotalHealthCost();
        UIPlayerInfo.Instance.UpdateMaxHearts();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        dashAction.Disable();
        mainAttackAction.Disable();

        health.onDamage -= (DamageSummary damage, float curr) => UIPlayerInfo.Instance.UpdateHealth();
        health.onDamage -= ActivateGloryTime;
    }

    public void ActivateGloryTime(DamageSummary damage, float curr)
    {
        if (curr > 0)
        {
            health.InvulnerableTime = invulnerabilityAfterHit;
        }
    }

    // States
    public void InitializeStates()
    {
        _idleState = new PIdleState(this);
        _moveState = new PMoveState(this);
        _dashState = new PDashState(this);
        _mainAttackState = new PMainAttack(this);

        stateMachine.ChangeState(_idleState);

        _idleState.onMove += HandleMove;
        _idleState.onMainAttack += HandleMainAttack;

        _moveState.onStop += HandleIdle;
        _moveState.onDash += HandleDash;
        _moveState.onMainAttack += HandleMainAttack;

        _mainAttackState.onFinish += HandleMove;

        _dashState.onFinish += HandleMove;

    }

    public void HandleIdle()
    {
        stateMachine.ChangeState(_idleState);
    }
    public void HandleMove()
    {
        stateMachine.ChangeState(_moveState);
    }
    public void HandleDash()
    {
        stateMachine.ChangeState(_dashState);
    }
    public void HandleMainAttack()
    {
        stateMachine.ChangeState(_mainAttackState);
    }

}
