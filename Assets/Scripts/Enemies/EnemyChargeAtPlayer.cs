using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyChargeAtPlayer : Enemy
{
    public Vector2 stompShakePower = Vector2.up * 1;
    public float minJumpDistance = 5f;
    public List<Transform> jumpPositions = new List<Transform>();

    public float wantedDistance = 0.1f;
    public float timeBetweenAttacks = 2f;
    public DamageSummary chargeDamage;
    public Collider2D chargeCollider;

    public Projectile blastPrefab;
    public Projectile stompPrefab;


    protected EIdleState idleState;
    protected EChargeAtPointState chargeState;
    protected EBlastAreaState shakeGroundState;
    protected EJumpAroundState jumpAroundState;

    private void OnEnable()
    {
        InitiateStates();
        UpdateUI();
    }

    private void UpdateUI()
    {
        BossInfo.Instance.Load(this);
    }

    public override void InitiateStates()
    {
        chargeState = new EChargeAtPointState(this, wantedDistance);
        idleState = new EIdleState(this, timeBetweenAttacks);
        shakeGroundState = new EBlastAreaState(this, blastPrefab);
        jumpAroundState = new EJumpAroundState(this, moveController.speed * 2f, 1f);

        chargeState.damageTouch = new DamageTouch(chargeDamage, chargeCollider);

        chargeState.onReachTarget += (Vector3 oldTarget) => ShakeGround();
        chargeState.onFailToReachTarget += ShakeGround;
        chargeState.AnimatorEventName = "Jump";

        jumpAroundState.onReachTargetPrefab = stompPrefab;
        jumpAroundState.onFinish += GoIdle;
        jumpAroundState.AnimatorEventName = "Jump";
        jumpAroundState.animatorStompEventName = "Stomp";

        shakeGroundState.onFinish += GoIdle;

        idleState.onFinish += PrepareAttack;

        GoIdle();
    }

    public void GoIdle()
    {
        stateMachine.ChangeState(idleState);
    }

    public void PrepareAttack()
    {
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            ChargeTarget();
        }
        else
        {
            JumpAround();
        }
    }

    public void ChargeTarget()
    {
        Player player = GameMaster.Instance.Player;
        if (player != null)
        {
            chargeState.Target = player.transform.position;
            stateMachine.ChangeState(chargeState);
        }
    }

    public void JumpAround()
    {
        List<Vector3> randomJumpPosition = new List<Vector3>();
        List<int> availableIndex = new List<int>();
        int index = 0;
        jumpPositions
            .Where(vector => Vector3.Distance(transform.position, vector.position) >= minJumpDistance).ToList()
            .ForEach((vector) => { availableIndex.Add(index); index++; });

        // We get at least one position thats threatning and remove it from the list
        int cheatClosestIndex = GetPositionIndexClosestToPlayer();
        availableIndex.Remove(cheatClosestIndex);

        int jumpsAmount = 2; // Home many random jumps we want in addition to cheat jump

        while (randomJumpPosition.Count < jumpsAmount && availableIndex.Count > 0)
        {
            // We select a random available index
            int randomInt = UnityEngine.Random.Range(0, availableIndex.Count);
            // We use the random index to get a random position from jump position
            randomJumpPosition.Add(jumpPositions[availableIndex[randomInt]].position);
            availableIndex.Remove(randomInt);
        }
        jumpAroundState.jumpTargets = randomJumpPosition;
        // We add the cheat jump posiion randomly in the array
        index = UnityEngine.Random.Range(1, randomJumpPosition.Count);
        randomJumpPosition.Insert(index, jumpPositions[cheatClosestIndex].position);

        // We change to jumping
        stateMachine.ChangeState(jumpAroundState);
    }

    protected int GetPositionIndexClosestToPlayer()
    {
        Player player = GameMaster.Instance.Player;
        Vector3 playerPos = player.transform.position;
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(jumpPositions[closestIndex].position, playerPos);
        for (int i = 1; i < jumpPositions.Count; i++)
        {
            float distance = Vector3.Distance(jumpPositions[i].position, playerPos);
            if (distance < closestDistance)
            {
                closestIndex = i;
                closestDistance = distance;
            }
        }
        return closestIndex;
    }

    public void ShakeGround()
    {
        StageCamera.Instance.Shake(Vector2.up);
        stateMachine.ChangeState(shakeGroundState);
    }
}
