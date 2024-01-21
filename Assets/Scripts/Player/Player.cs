using System;
using UnityEngine;
using UnityEngine.InputSystem;

public delegate void OnProjectileSpawn(Projectile projectile);

public class Player : Entity
{
    public float baseHealth = 10f;
    public Health specialBar;
    public PlayerWeapons playerWeapons;
    public PlayerInputs playerInput;

    public float specialChargeGainedPerHit = 0.25f;
    public float specialChargeGainedPerHealthLost = 1f;
    public float invulnerabilityAfterHit = 0.5f;
    [Header("Dash")]
    public float dashInvulnerability = 0.7f;
    public float dashDistance = 2f;
    public float dashDuration = 1f;
    public float dashCooldown = 0.25f;
    public AudioData dashSound;

    public InputAction moveAction;
    public InputAction dashAction;
    public InputAction mainAttackAction;
    public InputAction specialAttackAction;

    protected PIdleState _idleState;
    protected PMoveState _moveState;
    protected PDashState _dashState;
    protected PMainAttack _mainAttackState;
    protected PSpecialAttack _specialAttackState;
    protected PListening _listenState;
    protected PDeadState _deadState;

    protected Speaker speakerClose;
    protected float _timePassedFromDash = 0f;

    public Action onDashEnd;

    private void Awake()
    {
        playerInput = new PlayerInputs();
    }

    protected override void Update()
    {
        base.Update();
        _timePassedFromDash += Time.deltaTime;
    }

    private void OnEnable()
    {
        moveAction = playerInput.Player.Move;
        moveAction.Enable();
        dashAction = playerInput.Player.Dash;
        dashAction.Enable();
        mainAttackAction = playerInput.Player.Fire;
        mainAttackAction.Enable();
        specialAttackAction = playerInput.Player.Special;
        specialAttackAction.Enable();
        specialBar.Damage(new DamageSummary(9999));

        health.onDamage += HandleGetHit;
        health.onHeal += (float heal, float curr) => UIPlayerInfo.Instance.UpdateHealth();
        health.onDamage += ActivateGloryTime;
        health.onDeath += HandleDeath;
        specialBar.onDamage += (DamageSummary damage, float curr) => UIPlayerInfo.Instance.UpdateSpecialbar();
        specialBar.onHeal += (float heal, float curr) => UIPlayerInfo.Instance.UpdateSpecialbar();

        PowerManager.Instance.onPowerObtain += HandlePowerChange;
        PowerManager.Instance.onPowerLose += HandlePowerChange;

        InitializeStates();
    }

    public void HandleGetHit(DamageSummary damage, float curr)
    {
        UIPlayerInfo.Instance.UpdateHealth();
        specialBar.Heal(damage.damage * specialChargeGainedPerHealthLost, true);
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

        health.onDamage -= HandleGetHit;
        health.onHeal -= (float heal, float curr) => UIPlayerInfo.Instance.UpdateHealth();
        health.onDamage -= ActivateGloryTime;

        PowerManager.Instance.onPowerObtain -= HandlePowerChange;
        PowerManager.Instance.onPowerLose -= HandlePowerChange;
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
        _listenState = new PListening(this);
        _specialAttackState = new PSpecialAttack(this);
        _deadState = new PDeadState(this, 2f);

        stateMachine.ChangeState(_idleState);

        _idleState.onMove += HandleMove;
        _idleState.onMainAttack += HandleMainAttack;
        _idleState.onSpecialAttack += HandleSpecialAttack;

        _moveState.onStop += HandleIdle;
        _moveState.onDash += HandleDash;
        _moveState.onMainAttack += HandleMainAttack;
        _moveState.onSpecialAttack += HandleSpecialAttack;

        _mainAttackState.onFinish += HandleMove;
        _specialAttackState.onFinish += HandleMove;

        _dashState.onFinish += HandleDashEnd;

    }

    public void HandleIdle()
    {
        stateMachine.ChangeState(_idleState);
    }
    public void HandleMove()
    {
        if (Vector2.Distance(moveAction.ReadValue<Vector2>(), Vector2.zero) == 0)
            HandleIdle();
        else
            stateMachine.ChangeState(_moveState);
    }
    public void HandleDash()
    {
        if (_timePassedFromDash >= dashCooldown)
        {
            AudioMaster.Instance.PlaySoundEffect(dashSound);
            stateMachine.ChangeState(_dashState);
        }
    }
    public void HandleDashEnd()
    {
        _timePassedFromDash = 0;
        onDashEnd?.Invoke();
        HandleMove();
    }
    public void HandleMainAttack()
    {
        if (speakerClose)
        {
            speakerClose.StartSpeak();
        }
        else
        {
            stateMachine.ChangeState(_mainAttackState);
        }
    }

    public void HandleSpecialAttack()
    {
        if (specialBar.CurrentHealth >= specialBar.MaxHealth)
        {
            stateMachine.ChangeState(_specialAttackState);
            specialBar.Damage(new DamageSummary(9999));
        }
    }

    public void HandleDeath()
    {
        stateMachine.ChangeState(_deadState);
    }

    public void StartListening()
    {
        stateMachine.ChangeState(_listenState);
    }

    public void StopListening()
    {
        HandleIdle();
    }

    public void Revive()
    {
        stateMachine.ChangeState(_idleState);
    }

    public Speaker SpeakerClose { get { return speakerClose; } set { speakerClose = value; } }

}
