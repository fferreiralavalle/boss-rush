using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public bool isNotBoss = false;

    public float dangerHealth = 0.5f;
    public List<GameObject> activateOnDanger = new List<GameObject>();
    public AudioData specialAttackSound;

    protected EDeath _deathState;
    protected EIdleState _inactiveState;
    protected bool activatedDangerMode = false;
    protected bool didSpecial = false;

    public override void InitiateStates()
    {
        base.InitiateStates();
        _deathState = new EDeath(this);
        _inactiveState = new EIdleState(this, 99999999);

        GameMaster.Instance.Player.health.onDeath += Deactivate;
        health.onDamage += CheckIfReachedTreshold;
        float bossesDefeated = GameMaster.Instance.GetGameStateVariableValue<float>("BossesDefeated");
        didSpecial = bossesDefeated <= 0; // First boss wont do special move
    }

    public virtual void OnEnable()
    {
        health.onDeath += HandleDeath;
    }

    public virtual void HandleDeath()
    {
        stateMachine.ChangeState(_deathState);
        Utils.DeleteAllProjectiles();
    }

    public void Deactivate()
    {
        stateMachine.ChangeState(_inactiveState);
        Utils.DeleteAllProjectiles();
    }

    public void CheckIfReachedTreshold(DamageSummary dam, float currentHealth)
    {
        if (IsInDangerHealth)
        {
            OnDangerhealthReached();
            health.onDamage -= CheckIfReachedTreshold;
        }
    }

    public virtual void OnDangerhealthReached()
    {
        foreach(GameObject obj in activateOnDanger)
        {
            obj.SetActive(true);
        }
    }

    public virtual bool IsInDangerHealth {
        get {
            return health.HealthPercentage <= dangerHealth;
        }
    }
}
