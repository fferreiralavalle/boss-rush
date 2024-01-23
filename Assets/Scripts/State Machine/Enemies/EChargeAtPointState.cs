using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void OnReachPoint(Vector3 position);

public class EChargeAtPointState : EnemyState
{
    public event OnReachPoint onReachTarget;
    public event OnFailToReachTarget onFailToReachTarget;

    public float timeToReachBeforeGiveup = 5f;
    public float speedMultiplier = 1.25f;

    protected Vector3 _target;
    protected float _wantedDistance;

    public EChargeAtPointState(Enemy enemy, float wantedDistance, DamageTouch damageTouch) : base(enemy)
    {
        animatorEventName = "Charge";
        _wantedDistance = wantedDistance;
        this.damageTouch = damageTouch;
    }

    public override void Enter()
    {
        damageTouch.hitbox.enabled = true;
        _timePassed = 0;
        damageTouch.hitEntities = new List<Entity>();
        base.Enter();
    }

    public override void Leave()
    {
        damageTouch.hitbox.enabled = false;
        base.Leave();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (Target == null)
        {
            onFailToReachTarget?.Invoke();
            return;
        }
        _enemy.moveController.MoveTowards(_target, _enemy.moveController.speed * speedMultiplier);
        UpdateAnimatorDirection(_enemy, _target - _enemy.transform.position);
        float distance = Vector2.Distance(_target, _enemy.transform.position);
        if (distance < _wantedDistance)
        {
            onReachTarget?.Invoke(_target);
        }
        _timePassed += Time.fixedDeltaTime;
        if (_timePassed > timeToReachBeforeGiveup)
        {
            onFailToReachTarget?.Invoke();
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.GetComponent<Entity>();
        if (entity != null && entity.health.healthType == HealthType.Player && damageTouch.hitbox.IsTouching(collision) && !damageTouch.hitEntities.Contains(entity))
        {
            entity.health.Damage(damageTouch.onHitDamage);
            damageTouch.hitEntities.Add(entity);
        }
        base.OnTriggerEnter2D(collision);
    }

    public Vector3 Target { get { return _target; } set { _target = value; } }
}
