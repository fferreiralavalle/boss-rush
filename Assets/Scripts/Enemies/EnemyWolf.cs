using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWolf : Enemy
{
    public float timeBetweenAttacks = 2f;

    [Header("Charge Attack")]
    public float prepareTime = 0.4f;
    public float wantedDistance = 0.1f;
    public DamageSummary chargeDamage;
    public Collider2D chargeCollider;
    public AudioData chargeAudio;
    public int chargeWaveAmount = 3;

    [Header("Shadow Wolf Attack")]
    public Projectile shadowWolfPrefab;
    public int shadowsAmount = 3;
    public float timeBetweenShadows = 0.25f;
    public List<Transform> shadowSpawnPositions = new List<Transform>();

    [Header("Howl Attack")]
    public Projectile howlWavePrefab;
    public GameObject warningHowl;
    public List<Transform> howlPositions = new List<Transform>();
    public float howlWarningTime = 0.5f;
    public int howlWaves = 3;
    public float timeBetweenHowlWaves = 0.3f;
    public AudioData howlSound;

    [Header("Special - Shadow Chasers")]
    public int specialWaves = 3;
    public float timeBetweenSpecialWaves = 0.15f;
    public float minSpawnPlayerDistance = 4f;

    protected EIdleState idleState;
    // Charge State
    protected EIdleState prepareToChargeState;
    protected EChargeAtPointState chargeState;
    // Howl State
    protected EIdleState prepareHowl;
    protected EMoveToPosition moveToHowlPositionState;
    protected EBlastAreaState howlState;
    // Shadow State
    protected EMoveToPosition moveToShadowAttackState;
    protected EFireProjectilesAtPosition shadowAttackState;
    // Special State
    protected EMoveToPosition moveToSpecialAttackPosition;
    protected EFireProjectilesAtPosition specialAttackState;

    protected int _attackWavesAmount = 0;
    protected List<Transform> shadowAttackPositionsUsed;

    public override void OnEnable()
    {
        InitiateStates();
        UpdateUI();
        base.OnEnable();
    }

    private void UpdateUI()
    {
        BossInfo.Instance.Load(this);
    }

    public override void InitiateStates()
    {
        base.InitiateStates();
        idleState = new EIdleState(this, timeBetweenAttacks);
        // Howl
        howlState = new EBlastAreaState(this, howlWavePrefab);
        moveToHowlPositionState = new EMoveToPosition(this, howlPositions[0].position);
        prepareHowl = new EIdleState(this, howlWarningTime);
        prepareHowl.AnimatorEventName = "Prepare";
        howlState.AnimatorEventName = "Howl";

        moveToHowlPositionState.onFinish += PrepareHowl;
        prepareHowl.onFinish += HowlAttack;
        howlState.onFinish += HandleHowlEnd;

        // Shadows
        moveToShadowAttackState = new EMoveToPosition(this, shadowSpawnPositions[0].position);
        shadowAttackState = new EFireProjectilesAtPosition(this, shadowWolfPrefab, shadowsAmount, shadowSpawnPositions[0].position, timeBetweenShadows);
        shadowAttackState.rotateToLookAtTarget = false;
        shadowAttackState.targetDistance = 0;
        shadowAttackState.AnimatorEventName = "Howl";

        moveToShadowAttackState.onFinish += FirstShadowAttack;
        shadowAttackState.onFinish += HandleShadowAttackEnd;

        // Charge State
        chargeState = new EChargeAtPointState(this, wantedDistance, new DamageTouch(chargeDamage, chargeCollider));
        prepareToChargeState = new EIdleState(this, prepareTime);
        prepareToChargeState.onFinish += ChargeTarget;
        prepareToChargeState.AnimatorEventName = "PrepareCharge";

        chargeState.damageTouch = new DamageTouch(chargeDamage, chargeCollider);
        chargeState.onReachTarget += (Vector3 oldTarget) => HandleChargeReach();
        chargeState.onFailToReachTarget += HandleChargeReach;
        chargeState.AnimatorEventName = "Move";

        // Special
        moveToSpecialAttackPosition = new EMoveToPosition(this, shadowSpawnPositions[0].position);
        specialAttackState = new EFireProjectilesAtPosition(this, shadowWolfPrefab, 1, shadowSpawnPositions[0].position, timeBetweenSpecialWaves);
        specialAttackState.rotateToLookAtTarget = false;
        specialAttackState.targetDistance = 0;
        specialAttackState.AnimatorEventName = "Howl";

        moveToSpecialAttackPosition.onFinish += FireSpecialAttack;
        specialAttackState.onFinish += HandleSpecialAttackWaveEnd;

        idleState.onFinish += PrepareAttack;

        GoIdle();
    }

    public void GoIdle()
    {
        idleState.idleTime = timeBetweenAttacks * (IsInDangerHealth ? 0.5f : 1);
        stateMachine.ChangeState(idleState);
    }

    public void PrepareAttack()
    {
        if (IsInDangerHealth && !didSpecial)
        {
            MoveToSpecialAttackPosition();
            AudioMaster.Instance.PlaySoundEffect(specialAttackSound);
            didSpecial = true;
            return;
        }
        int random = UnityEngine.Random.Range(0, 3);
        switch(random)
        {
            default:
                MoveToShadowAttackPosition();
                break;
            case 1:
                GoToHowlPosition();
                break;
            case 2:
                PrepareToCharge();
                break;
        }
    }

    // Special Attack
    public void MoveToSpecialAttackPosition()
    {
        int index = Utils.GetPositionIndexFarthestToPlayer(shadowSpawnPositions);
        shadowAttackPositionsUsed = new List<Transform>() { shadowSpawnPositions[index] };
        moveToSpecialAttackPosition.position = shadowAttackPositionsUsed[0].position;
        stateMachine.ChangeState(moveToSpecialAttackPosition);
    }

    public void FireSpecialAttack()
    {
        AudioMaster.Instance.PlaySoundEffect(howlSound);
        LookAtPlayer();
        Vector3 playerPos = GameMaster.Instance.Player.transform.position;
        List<Transform> shadows = shadowSpawnPositions.Where(
            pos => !shadowAttackPositionsUsed.Contains(pos) &&
            Vector3.Distance(playerPos, pos.position) >= minSpawnPlayerDistance
            ).ToList();
        Transform randomPos = shadows[UnityEngine.Random.Range(0, shadows.Count)];
        shadowAttackState.targetPosition = randomPos.position;
        specialAttackState.targetPosition = randomPos.position;
        stateMachine.ChangeState(specialAttackState);
    }

    public void HandleSpecialAttackWaveEnd()
    {
        _attackWavesAmount++;
        if (_attackWavesAmount < specialWaves)
        {
            FireSpecialAttack();
        }
        else
        {
            _attackWavesAmount = 0;
            GoIdle();
        }
    }

    public void PrepareToCharge()
    {
        AudioMaster.Instance?.PlaySoundEffect(chargeAudio);
        stateMachine.ChangeState(prepareToChargeState);
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

    public void HandleChargeReach()
    {
        LookAtPlayer();
        _attackWavesAmount++;
        if (_attackWavesAmount < chargeWaveAmount)
        {
            PrepareToCharge();
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

    public void PrepareHowl()
    {
        AudioMaster.Instance.PlaySoundEffect(howlSound);
        Instantiate(warningHowl, transform);
        stateMachine.ChangeState(prepareHowl);
    }

    public void HowlAttack()
    {
        HowlCameraEffect();
        stateMachine.ChangeState(howlState);
    }

    public void HandleHowlEnd()
    {
        _attackWavesAmount++;
        if (_attackWavesAmount < (howlWaves + (IsInDangerHealth ? 1 : 0)))
        {
            HowlAttack();
        }
        else
        {
            _attackWavesAmount = 0;
            GoIdle();
        }
    }

    public void MoveToShadowAttackPosition()
    {
        int index = Utils.GetPositionIndexFarthestToPlayer(shadowSpawnPositions);
        shadowAttackPositionsUsed = new List<Transform>(){ shadowSpawnPositions[index] };
        moveToShadowAttackState.position = shadowAttackPositionsUsed[0].position;
        stateMachine.ChangeState(moveToShadowAttackState);
    }

    public void FirstShadowAttack()
    {
        AudioMaster.Instance.PlaySoundEffect(howlSound);
        ShadowAttack();
    }

    public void ShadowAttack()
    {
        LookAtPlayer();
        List<Transform> shadows = shadowSpawnPositions.Where(pos => !shadowAttackPositionsUsed.Contains(pos)).ToList();
        int farthestAwayIndex = Utils.GetPositionIndexFarthestToPlayer(shadows);
        shadowAttackState.targetPosition = shadows[farthestAwayIndex].position;
        shadowAttackPositionsUsed.Add(shadows[farthestAwayIndex]);
        stateMachine.ChangeState(shadowAttackState);
    }

    public void HandleShadowAttackEnd()
    {
        _attackWavesAmount++;
        if (_attackWavesAmount < (shadowsAmount + (IsInDangerHealth ? 1 : 0)))
        {
            ShadowAttack();
        }
        else
        {
            _attackWavesAmount = 0;
            GoIdle();
        }
    }

    public void HowlCameraEffect()
    {
        StageCamera.Instance.Shake(Vector2.right);
    }

    public void LookAtPlayer()
    {
        Player player = GameMaster.Instance.Player;
        Vector3 dir = player.transform.position - animator.transform.position;
        animator.transform.localScale = new Vector3(
            Mathf.Sign(dir.x) * Mathf.Abs(animator.transform.localScale.x),
            animator.transform.localScale.y,
            animator.transform.localScale.z
        );
    }
}
