using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBlastAreaState : EnemyState
{
    public float duration = 1f;

    public Projectile blastPrefab;

    protected Projectile blastInstance;

    public EBlastAreaState(Enemy enemy, Projectile blastPrefab) : base(enemy)
    {
        this.blastPrefab = blastPrefab;
        animatorEventName = "Blast";
    }

    public override void Enter()
    {
        blastInstance = Object.Instantiate(blastPrefab);
        blastInstance.Initiate(_enemy);
        blastInstance.transform.position = _enemy.transform.position;
        base.Enter();
    }

    public override void OnUpdate()
    {
        _timePassed += Time.deltaTime;
        base.OnUpdate();
        if (_timePassed > duration )
        {
            HandleFinish();
        }
    }


}
