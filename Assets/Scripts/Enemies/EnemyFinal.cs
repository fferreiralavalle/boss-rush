using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnemyFinal : Enemy
{
    public float timeBetweenAttacks = 2f;

    [Header("Orbit Attack")]
    public Projectile orbitAttackPrefab;
    public Transform orbitSpawnPosition;
    public int orbitProjectileAmount = 12;
    public int orbitWaveAmount = 4;
    public float orbitAttackDuration = 2;
    public List<Transform> skyPositions = new List<Transform>();
    public List<Transform> orbitAttackPositions = new List<Transform>();

    [Header("Commet Attack")]
    public Projectile commetPrefab;
    public int commetAmount = 1;
    public int commetWavesAmount = 3;
    public float commetAttackDuration = 2;
    public float commetDistanceFromTarget = 7f;
    public float degreesFallDirection = 70f;
    public List<Transform> commetAttackPositions = new List<Transform>();
    public List<Transform> commetFallPositions = new List<Transform>();

    [Header("Laser Attack")]
    public Projectile laserPrefab;
    public int laserAmount = 6;
    public float laserAttackDuration = 3;
    public List<Transform> laserAttackPositions = new List<Transform>();

    [Header("Special Attack")]
    public float specialAttackDuration = 4f;
    public VolumeProfile dangerVolume;
    public Light2D turnOffLight;
    public Light2D turnOnLight;
    public AudioData finalMusic;

    protected EIdleState idleState;

    // Orbit Attack States
    protected EOrbitProjectile orbitProjectileState;
    protected EMoveToPosition moveToOrbitProyectilePositionState;

    // Commet Attack States
    protected EFireProjectilesAtPosition commetAttackState;
    protected EMoveToPosition moveToCommetPositionState;

    // Mega Laser State
    protected EMoveToPosition moveToLaserPosition;
    protected ELaserProjectileState laserProjectileState;

    // Special Attack
    protected EMoveToPosition moveToSpecialAttackPositionState;

    protected int orbitAttacksInARow = 0;

    public override void OnEnable()
    {
        base.OnEnable();
        InitiateStates();
        UpdateUI();
    }

    private void UpdateUI()
    {
        BossInfo.Instance.Load(this);
    }

    public override void InitiateStates()
    {
        idleState = new EIdleState(this, timeBetweenAttacks);

        // Orbit Attack
        orbitProjectileState = new EOrbitProjectile(this, orbitAttackPrefab, orbitProjectileAmount, orbitSpawnPosition, orbitAttackDuration);
        moveToOrbitProyectilePositionState = new EMoveToPosition(this, skyPositions[0].position, 2f);
        orbitProjectileState.AnimatorEventName = "Attack";

        orbitProjectileState.onFinish += HandleOrbitAttackEnd;
        moveToOrbitProyectilePositionState.onFinish += FireOrbitAttackWaves;

        // Commet Attack
        commetAttackState = new EFireProjectilesAtPosition(this, commetPrefab, commetAmount, commetFallPositions[0].position, commetAttackDuration);
        moveToCommetPositionState = new EMoveToPosition(this, orbitAttackPositions[0].position);

        commetAttackState.targetDistance = commetDistanceFromTarget;
        commetAttackState.initialDegrees = degreesFallDirection;

        commetAttackState.onFinish += HandleCommetAttackEnd;
        moveToCommetPositionState.onFinish += FireCommetAttackWaves;

        // Mega Laser Attack
        laserProjectileState = new ELaserProjectileState(this, laserPrefab, laserAmount, orbitSpawnPosition, laserAttackDuration);
        moveToLaserPosition = new EMoveToPosition(this, laserAttackPositions[0].position);

        laserProjectileState.onFinish += GoIdle;
        moveToLaserPosition.onFinish += FireLaserAttac;

        idleState.onFinish += PrepareAttack;

        // Special Attack
        moveToSpecialAttackPositionState = new EMoveToPosition(this, orbitAttackPositions[0].position);
        moveToSpecialAttackPositionState.onFinish += AwaitSpecial;

        GoIdle();
        base.InitiateStates();
    }

    public void GoIdle()
    {
        idleState.idleTime = TimeBetweenAttacks;
        stateMachine.ChangeState(idleState);
    }

    public void PrepareAttack()
    {
        if (IsInDangerHealth && !didSpecial)
        {
            didSpecial = true;
            AudioMaster.Instance.PlayMusic(finalMusic);
            AudioMaster.Instance.PlaySoundEffect(specialAttackSound);
            MoveToSpecialAttackPosition();
            return;
        }
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 1:
                MoveToOrbitAttackPosition();
                break;
            case 2:
                MoveTolaserAttackPosition();
                break;
            default:
                MoveToCommetAttackPosition();
                break;
        }
    }

    public void MoveToSpecialAttackPosition()
    {
        stateMachine.ChangeState(moveToSpecialAttackPositionState);
    }

    public void AwaitSpecial()
    {
        // Add mid battle cutscene
        idleState.idleTime = specialAttackDuration;
        Camera.main.GetComponent<Volume>().profile = dangerVolume;
        turnOffLight.gameObject.SetActive(false);
        turnOnLight.gameObject.SetActive(true);
        stateMachine.ChangeState(idleState);
    }

    public void MoveToOrbitAttackPosition()
    {
        int farthestIndex = Utils.GetPositionIndexFarthestToPlayer(skyPositions);
        moveToOrbitProyectilePositionState.position = skyPositions[farthestIndex].position;
        stateMachine.ChangeState(moveToOrbitProyectilePositionState);
    }

    public void FireOrbitAttackWaves()
    {
        stateMachine.ChangeState(orbitProjectileState);
    }

    public void HandleOrbitAttackEnd()
    {
        orbitAttacksInARow++;
        int orbitWaves = orbitWaveAmount + (IsInDangerHealth ? 1 : 0);
        if (orbitAttacksInARow < orbitWaveAmount)
        {
            orbitProjectileState.maxDistanceMod = -1 * orbitAttacksInARow;
            FireOrbitAttackWaves();
        }
        else
        {
            orbitAttacksInARow = 0;
            orbitProjectileState.maxDistanceMod = 0f;
            GoIdle();
        }
    }

    public void MoveToCommetAttackPosition()
    {
        int farthestIndex = Utils.GetPositionIndexFarthestToPlayer(commetAttackPositions);
        moveToCommetPositionState.position = commetAttackPositions[farthestIndex].position;
        stateMachine.ChangeState(moveToCommetPositionState);
    }

    public void FireCommetAttackWaves()
    {
        stateMachine.ChangeState(commetAttackState);
    }

    public void HandleCommetAttackEnd()
    {
        orbitAttacksInARow++;
        ShakeScreen();
        int wavesAmount = commetWavesAmount + (IsInDangerHealth ? 2 : 0);
        if (orbitAttacksInARow < wavesAmount)
        {
            int index = Random.Range(0, commetFallPositions.Count);
            commetAttackState.targetPosition = commetFallPositions[index].position;
            FireCommetAttackWaves();
        }
        else
        {
            orbitAttacksInARow = 0;
            GoIdle();
        }
    }

    public void MoveTolaserAttackPosition()
    {
        int farthestIndex = Utils.GetPositionIndexFarthestToPlayer(laserAttackPositions);
        moveToCommetPositionState.position = laserAttackPositions[farthestIndex].position;
        stateMachine.ChangeState(moveToLaserPosition);
    }

    public void FireLaserAttac()
    {
        laserProjectileState.spawnAmount = laserAmount + (IsInDangerHealth ? 2 : 0);
        stateMachine.ChangeState(laserProjectileState);
    }

    public void ShakeScreen()
    {
        StageCamera.Instance.Shake(Vector2.up);
    }

    public float TimeBetweenAttacks
    {
        get
        {
            if (IsInDangerHealth)
                return timeBetweenAttacks * 0.5f;
            return timeBetweenAttacks;
        }
    }
}
