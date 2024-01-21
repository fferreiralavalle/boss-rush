using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EJumpAroundState : EnemyState
{
    public List<Vector3> jumpTargets = new List<Vector3>();
    public float jumpSpeed = 3f;
    public float stayInTargetDuration = .2f;
    public Projectile onReachTargetPrefab;
    public int index = 0;
    public string animatorStompEventName = "JumpAround";

    protected bool jumping = true;

    public EJumpAroundState(Enemy enemy, float jumpSpeed, float stayInTargetDuration) : base(enemy)
    {
        this.jumpSpeed = jumpSpeed;
        this.stayInTargetDuration = stayInTargetDuration;
        animatorEventName = "JumpAround";
    }

    public override void Enter()
    {
        jumping = true;
        index = 0;
        base.Enter();
    }

    public override void OnFixedUpdate()
    {
        if (jumping && index < jumpTargets.Count)
        {
            Vector3 currentTarget = jumpTargets[index];
            MoveTowards(currentTarget);
            UpdateAnimatorDirection(_enemy, currentTarget - _enemy.transform.position);
            if (Vector3.Distance(_enemy.transform.position, currentTarget) == 0)
            {
                jumping = false;
            }
        }
        else
        {
            Stomp();
        }
        base.OnFixedUpdate();
    }

    public void MoveTowards(Vector3 target)
    {
        _enemy.moveController.MoveTowards(target, jumpSpeed);
    }

    public void Stomp()
    {
        // The first time we enter
        if (_timePassed == 0)
        {
            Projectile proyectile = Object.Instantiate(onReachTargetPrefab).Initiate(_enemy);
            proyectile.transform.position = _enemy.transform.position;
            _enemy.animator.SetBool(animatorStompEventName, true);
        }
        _timePassed += Time.fixedDeltaTime;
        // We a bit and then we jump again
        if (_timePassed > stayInTargetDuration)
        {
            StageCamera.Instance.Shake(Vector2.up * 0.5f);
            _enemy.animator.SetBool(animatorStompEventName, false);
            jumping = true;
            _timePassed = 0;
            index++;
            if (index >= jumpTargets.Count)
            {
                HandleFinish();
            }
        }
    }
}
