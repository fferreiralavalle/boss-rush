using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class EBanishAndReappearState : EnemyState
{
    public Vector3 target = Vector3.zero;
    public Vector3 distanceFromTarget;
    public Projectile onAppearPrefab;
    public GameObject targetPreviewPrefab;
    public float timeToReppear = 1f;
    public float speedMultiplier = 1.5f;

    protected GameObject warningInstance;
    protected bool dontReadjustPosition = false;

    public Action onLand;

    public EBanishAndReappearState(Enemy enemy, Vector3 target, Vector3 distanceFromTarget) : base(enemy)
    {
        this.target = target;
        this.distanceFromTarget = distanceFromTarget;
    }

    public EBanishAndReappearState(Enemy enemy, Vector3 target) : base(enemy)
    {
        this.target = target;
        dontReadjustPosition = true;
    }


    public override void Enter()
    {
        if (!dontReadjustPosition)
        {
            _enemy.transform.position = target + distanceFromTarget;
        }
        _timePassed = 0;
        warningInstance = Object.Instantiate(targetPreviewPrefab);
        warningInstance.transform.position = target;
        base.Enter();
    }

    public override void Leave()
    {
        base.Leave();
        if (warningInstance)
            Object.Destroy(warningInstance);
    }

    public override void OnFixedUpdate()
    {
        _timePassed += Time.fixedDeltaTime;
        base.OnFixedUpdate();
        if (_timePassed > timeToReppear)
        {
            _enemy.moveController.MoveTowards(target, _enemy.moveController.speed * speedMultiplier);
            UpdateAnimatorDirection(_enemy, target - _enemy.transform.position);
            float distance = Vector2.Distance(target, _enemy.transform.position);
            if (distance == 0)
            {
                Projectile projectile = Object.Instantiate(onAppearPrefab).Initiate(_enemy);
                projectile.transform.position = target;
                onLand?.Invoke();
                HandleFinish();
            }
        }
    }
}
