using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyChargeAtPlayer : Enemy
{
    public float timeBetweenAttacks = 2f;
    public Vector2 stompShakePower = Vector2.up * 1;
    public float minJumpDistance = 5f;
    public List<Transform> jumpPositions = new List<Transform>();

    [Header("Charge Attack")]
    public float prepareTime = 0.4f;
    public float wantedDistance = 0.1f;
    public DamageSummary chargeDamage;
    public Collider2D chargeCollider;
    public AudioData chargeAudio;
    public int chargeWaveAmount = 3;
    
    public Projectile stompPrefab;

    [Header("Laser Attack")]
    public Projectile laserPrefab;
    public int laserAmount = 3;
    public float laserAttackDuration = 5f;
    public Transform laserSpawnPosition;

    [Header("Howl Attack")]
    public Projectile howlWavePrefab;
    public List<Transform> howlPositions = new List<Transform>();
    public int howlWaves = 3;
    public float timeBetweenWaves = 0.3f;

    protected EIdleState idleState;
    // Charge State
    protected EIdleState prepareToChargeState;
    protected EChargeAtPointState chargeState;
    // Howl State
    protected EMoveToPosition moveToHowlPositionState;
    protected EBlastAreaState howlState;
    // Others
    protected EJumpAroundState jumpAroundState;
    protected ELaserProjectileState laserProjectileState;

    protected int _attackWavesAmount = 0;

    public override void OnEnable()
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
        base.InitiateStates();
        chargeState = new EChargeAtPointState(this, wantedDistance, new DamageTouch(chargeDamage, chargeCollider));
        idleState = new EIdleState(this, timeBetweenAttacks);
        // Howl
        howlState = new EBlastAreaState(this, howlWavePrefab);
        moveToHowlPositionState = new EMoveToPosition(this, howlPositions[0].position);

        moveToHowlPositionState.onFinish += HowlAttack;
        howlState.onFinish += HandleHowlEnd;

        jumpAroundState = new EJumpAroundState(this, moveController.speed * 2f, 1f);
        laserProjectileState = new ELaserProjectileState(this, laserPrefab, laserAmount, laserSpawnPosition, laserAttackDuration);

        // Charge State
        prepareToChargeState = new EIdleState(this, prepareTime);
        prepareToChargeState.onFinish += ChargeTarget;
        prepareToChargeState.AnimatorEventName = "Prepare";

        chargeState.damageTouch = new DamageTouch(chargeDamage, chargeCollider);
        chargeState.onReachTarget += (Vector3 oldTarget) => HandleChargeReach();
        chargeState.onFailToReachTarget += HandleChargeReach;
        chargeState.AnimatorEventName = "Jump";

        jumpAroundState.onReachTargetPrefab = stompPrefab;
        jumpAroundState.onFinish += GoIdle;
        jumpAroundState.AnimatorEventName = "Jump";
        jumpAroundState.animatorStompEventName = "Stomp";

        laserProjectileState.onFinish += GoIdle;

        idleState.onFinish += PrepareAttack;

        GoIdle();
    }

    public void GoIdle()
    {
        stateMachine.ChangeState(idleState);
    }

    public void PrepareAttack()
    {
        int random = UnityEngine.Random.Range(0, 3);
        switch(random)
        {
            default:
                PrepareToCharge();
                break;
            case 1:
                GoToHowlPosition();
                break;
            case 2:
                LaserAttack();
                break;
        }
    }

    public void PrepareToCharge()
    {
        stateMachine.ChangeState(prepareToChargeState);
    }

    public void ChargeTarget()
    {
        Player player = GameMaster.Instance.Player;
        if (player != null)
        {
            AudioMaster.Instance?.PlaySoundEffect(chargeAudio);
            chargeState.Target = player.transform.position;
            stateMachine.ChangeState(chargeState);
        }
    }

    public void HandleChargeReach()
    {

        _attackWavesAmount++;
        if (_attackWavesAmount < chargeWaveAmount)
        {
            ChargeTarget();
        }
        else
        {
            _attackWavesAmount = 0;
            GoIdle();
        }
    }

    public void GoToHowlPosition()
    {
        stateMachine.ChangeState(moveToHowlPositionState);
    }

    public void HowlAttack()
    {
        stateMachine.ChangeState(howlState);
    }

    public void HandleHowlEnd()
    {
        _attackWavesAmount++;
        if (_attackWavesAmount < howlWaves)
        {
            HowlAttack();
        }
        else
        {
            _attackWavesAmount = 0;
            GoIdle();
        }
    }

    public void LaserAttack()
    {
        stateMachine.ChangeState(laserProjectileState);
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
        int cheatClosestIndex = Utils.GetPositionIndexClosestToPlayer(jumpPositions);
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

    public void HowlCameraEffect()
    {
        StageCamera.Instance.Shake(Vector2.right);
    }
}
