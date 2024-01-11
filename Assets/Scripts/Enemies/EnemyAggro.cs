using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : Enemy
{
    public float wantedDistance = 1f;
    protected EChaseState chaseState;

    private void OnEnable()
    {
        InitiateStates();
    }

    public override void InitiateStates()
    {
        chaseState = new EChaseState(this, wantedDistance);
    }

    private void Start()
    {
        ChaseTarget();
    }

    public void ChaseTarget()
    {
        Player player = GameMaster.Instance.Player;
        if (player != null)
        {
            chaseState.Target = player.transform;
            stateMachine.ChangeState(chaseState);
        }
    }
}
