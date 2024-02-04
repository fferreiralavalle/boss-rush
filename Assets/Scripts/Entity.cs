using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Health health;
    public Animator animator;
    public MoveController moveController;
    public StateMachine stateMachine;

    public virtual void InitiateStates()
    {

    }

    protected virtual void FixedUpdate()
    {
        stateMachine?.CurrentState?.OnFixedUpdate();
    }

    protected virtual void Update()
    {
        stateMachine?.CurrentState?.OnUpdate();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        stateMachine?.CurrentState?.OnTriggerEnter2D(collision);
    }
}
