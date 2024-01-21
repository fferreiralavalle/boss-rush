public class Enemy : Entity
{
    public bool isNotBoss = false;

    protected EDeath _deathState;
    protected EIdleState _inactiveState;

    public override void InitiateStates()
    {
        base.InitiateStates();
        _deathState = new EDeath(this);
        _inactiveState = new EIdleState(this, 99999999);

        GameMaster.Instance.Player.health.onDeath += Deactivate;
    }

    public virtual void OnEnable()
    {
        health.onDeath += HandleDeath;
    }

    public virtual void HandleDeath()
    {
        stateMachine.ChangeState(_deathState);
    }

    public void Deactivate()
    {
        stateMachine.ChangeState(_inactiveState);
    }
}
