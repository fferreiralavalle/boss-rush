using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnFinish();

[Serializable]
public class State
{
    public event OnFinish onFinish;

    protected float _timePassed = 0;

    public virtual void Enter()
    {
        _timePassed = 0f;
    }

    public virtual void Leave()
    {

    }

    // Functions to be called by parent
    public virtual void OnUpdate()
    {

    }
    public virtual void OnFixedUpdate()
    {

    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void UpdateAnimatorDirection(Entity entity, Vector2 direction)
    {
        entity.animator.transform.localScale = new Vector3(
            Mathf.Sign(direction.x) * Mathf.Abs(entity.animator.transform.localScale.x),
            entity.animator.transform.localScale.y,
            entity.animator.transform.localScale.z
            );
    }

    public void HandleFinish()
    {
        onFinish.Invoke();
    }
}
