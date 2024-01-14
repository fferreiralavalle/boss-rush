using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    public bool isNotBoss = false;

    protected EDeath deathState;

    public override void InitiateStates()
    {
        base.InitiateStates();
        deathState = new EDeath(this);
    }

    public virtual void OnEnable()
    {
        health.onDeath += HandleDeath;
    }

    public virtual void HandleDeath()
    {
        stateMachine.ChangeState(deathState);
    }
}
