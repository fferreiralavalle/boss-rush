using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyJump : Enemy
{
    public float timeBetweenAttacks = 2f;

    [Header("Vomit Swarm Attack")]
    public Projectile vomitAttackProjectile;
    public Transform vomitSpawnPosition;
    public int proyectileAmount = 5;
    public int vomitWaves = 3;
    public float vomitDuration = 2;
    public List<Transform> vomitStagePositions = new List<Transform>();

    [Header("Orbit Attack")]
    public Projectile orbitAttackPrefab;
    public Transform orbitSpawnPosition;
    public int orbitProjectileAmount = 12;
    public int orbitWaveAmount = 4;
    public float orbitAttackDuration = 2;
    public List<Transform> orbitAttackPositions = new List<Transform>();

    [Header("Attack From Sky")]
    public Projectile stompFromSkyPrefab;
    public GameObject previewStompPrefab;
    public Transform outOfScreenPosition;
    public float timeToReaper = 1f;
    public float flyUpSpeedMultiplier = 1.5f;
    public Vector3 offSkyAttack = Vector3.up * 5;
    public AudioData jumpSound;

    [Header("Jump Around")]
    public Projectile stompPrefab;
    public List<Transform> positions = new List<Transform>();

    [Header("SPECIAL - Orbit Attack")]
    public int specialProjectileAmount = 12;
    public int specialWaveAmount = 6;
    public float specialWaveDuration = 1f;
    public Projectile specialProjectile;
    public Transform specialAttackPosition;

    protected EIdleState idleState;
    protected EJumpAroundState jumpAroundState;

    // Orbit Attack States
    protected EOrbitProjectile orbitProjectileState;
    protected EMoveToPosition moveToOrbitProyectilePositionState;

    // Vomit Attack States
    protected ELineProjectiles throwProjectileState;
    protected EMoveToPosition moveToProyectileFirePositionState;

    // Attack from sky Attack States
    protected EMoveToPosition moveToAttackFromSkyPositionState;
    protected EBanishAndReappearState reappearState;

    // Special Attack States
    protected EOrbitProjectile specialAttackState;
    protected EMoveToPosition moveToSpecialPositionState;

    protected int _wavesAmount = 0;

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
        moveToOrbitProyectilePositionState = new EMoveToPosition(this, orbitAttackPositions[0].position, 2f);
        moveToOrbitProyectilePositionState.AnimatorEventName = "Jump";

        orbitProjectileState.onFinish += HandleOrbitAttackEnd;
        moveToOrbitProyectilePositionState.onFinish += FireOrbitAttackWaves;

        moveToProyectileFirePositionState = new EMoveToPosition(this, vomitStagePositions[0].position);
        throwProjectileState = new ELineProjectiles(this, vomitAttackProjectile, proyectileAmount, vomitSpawnPosition, vomitDuration);
        throwProjectileState.degreesSpawnArc = 60;
        throwProjectileState.spawnDelay = 0;
        throwProjectileState.spawnDistanceFromCenter = 0.3f;
        throwProjectileState.rotateToDirection = false;
        moveToProyectileFirePositionState.AnimatorEventName = "Jump";

        jumpAroundState = new EJumpAroundState(this, moveController.speed, 1f);

        moveToAttackFromSkyPositionState = new EMoveToPosition(this, outOfScreenPosition.position);
        moveToAttackFromSkyPositionState.speedMultiplier = flyUpSpeedMultiplier;
        reappearState = new EBanishAndReappearState(this, Vector2.zero);
        reappearState.timeToReppear = timeToReaper;
        reappearState.speedMultiplier = flyUpSpeedMultiplier;

        jumpAroundState.onReachTargetPrefab = stompPrefab;
        jumpAroundState.onFinish += GoIdle;
        jumpAroundState.AnimatorEventName = "Jump";
        jumpAroundState.animatorStompEventName = "Stomp";
        jumpAroundState.jumpSound = jumpSound;

        moveToProyectileFirePositionState.onFinish += VomitSwarm;
        throwProjectileState.onFinish += HandleVomitEnd;

        moveToAttackFromSkyPositionState.onFinish += FromSkyAttack;
        moveToAttackFromSkyPositionState.AnimatorEventName = "Jump";

        reappearState.onAppearPrefab = stompFromSkyPrefab;
        reappearState.targetPreviewPrefab = previewStompPrefab;
        reappearState.AnimatorEventName = "Fall";
        reappearState.onFinish += GoIdle;
        reappearState.onLand += ShakeScreen;

        idleState.onFinish += PrepareAttack;

        // Special
        specialAttackState = new EOrbitProjectile(this, specialProjectile, specialProjectileAmount, specialAttackPosition, specialWaveDuration);
        moveToSpecialPositionState = new EMoveToPosition(this, specialAttackPosition.position, 2f);

        moveToSpecialPositionState.onFinish += FireSpecialAttack;
        specialAttackState.onFinish += SpecialAttackEnd;

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
            AudioMaster.Instance.PlaySoundEffect(specialAttackSound);
            MoveToSpecialAttackPosition();
            didSpecial = true;
            return;
        }
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 1:
                MoveToVomitPosition();
                break;
            case 2:
                MoveToOrbitAttackPosition();
                break;
            default:
                MoveToSkyAttackPosition();
                break;
        }
    }

    public void MoveToSpecialAttackPosition()
    {
        stateMachine.ChangeState(moveToSpecialPositionState);
    }

    public void FireSpecialAttack()
    {
        specialAttackState.maxDistanceMod = -1 * _wavesAmount;
        stateMachine.ChangeState(specialAttackState);
    }

    public void SpecialAttackEnd()
    {
        _wavesAmount++;
        if (_wavesAmount < specialWaveAmount)
        {
            FireSpecialAttack();
        }
        else
        {
            _wavesAmount = 0;
            GoIdle();
        }
    }


    public void MoveToSkyAttackPosition()
    {
        AudioMaster.Instance.PlaySoundEffect(jumpSound);
        stateMachine.ChangeState(moveToAttackFromSkyPositionState);
    }

    public void FromSkyAttack()
    {
        reappearState.target = GameMaster.Instance.Player.transform.position;
        reappearState.timeToReppear = health.HealthPercentage < dangerHealth ? timeToReaper * 0.5f : timeToReaper;
        stateMachine.ChangeState(reappearState);
    }

    public void MoveToOrbitAttackPosition()
    {
        AudioMaster.Instance.PlaySoundEffect(jumpSound);
        int farthestIndex = Utils.GetPositionIndexFarthestToPlayer(orbitAttackPositions);
        moveToOrbitProyectilePositionState.position = orbitAttackPositions[farthestIndex].position;
        stateMachine.ChangeState(moveToOrbitProyectilePositionState);
    }

    public void FireOrbitAttackWaves()
    {
        stateMachine.ChangeState(orbitProjectileState);
    }

    public void HandleOrbitAttackEnd()
    {
        _wavesAmount++;
        if (_wavesAmount < orbitWaveAmount)
        {
            orbitProjectileState.maxDistanceMod = -1 * _wavesAmount;
            FireOrbitAttackWaves();
        }
        else
        {
            _wavesAmount = 0;
            orbitProjectileState.maxDistanceMod = 0f;
            JumpAround();
        }
    }

    public void MoveToVomitPosition()
    {
        AudioMaster.Instance.PlaySoundEffect(jumpSound);
        int index = GetPositionIndexFarthestToPlayer(vomitStagePositions);
        moveToProyectileFirePositionState.position = vomitStagePositions[index].position;
        stateMachine.ChangeState(moveToProyectileFirePositionState);
    }

    public void VomitSwarm()
    {
        throwProjectileState.spawnAmount = proyectileAmount;
        throwProjectileState.spawnDelay = vomitDuration * (IsInDangerHealth ? 0.75f : 0);
        stateMachine.ChangeState(throwProjectileState);
    }

    public void HandleVomitEnd()
    {
        _wavesAmount++;
        int finalWaves = vomitWaves + (IsInDangerHealth ? 4 : 0);
        if (_wavesAmount >= finalWaves)
        {
            _wavesAmount = 0;
            GoIdle();
        }
        else
        {
            VomitSwarm();
        }
    }

    public void JumpAround()
    {
        List<Vector3> randomJumpPosition = new List<Vector3>();
        List<int> availableIndex = new List<int>();
        int index = 0;
        positions
            .Where(vector => Vector3.Distance(transform.position, vector.position) >= 5).ToList()
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
            randomJumpPosition.Add(positions[availableIndex[randomInt]].position);
            availableIndex.Remove(randomInt);
        }
        jumpAroundState.jumpTargets = randomJumpPosition;
        // We add the cheat jump posiion randomly in the array
        index = UnityEngine.Random.Range(1, randomJumpPosition.Count);
        randomJumpPosition.Insert(index, positions[cheatClosestIndex].position);

        // We change to jumping
        stateMachine.ChangeState(jumpAroundState);
    }

    protected int GetPositionIndexClosestToPlayer()
    {
        Player player = GameMaster.Instance.Player;
        Vector3 playerPos = player.transform.position;
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(positions[closestIndex].position, playerPos);
        for (int i = 1; i < positions.Count; i++)
        {
            float distance = Vector3.Distance(positions[i].position, playerPos);
            if (distance < closestDistance)
            {
                closestIndex = i;
                closestDistance = distance;
            }
        }
        return closestIndex;
    }

    protected int GetPositionIndexFarthestToPlayer(List<Transform> positions)
    {
        Player player = GameMaster.Instance.Player;
        Vector3 playerPos = player.transform.position;
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(positions[closestIndex].position, playerPos);
        for (int i = 1; i < positions.Count; i++)
        {
            float distance = Vector3.Distance(positions[i].position, playerPos);
            if (distance > closestDistance)
            {
                closestIndex = i;
                closestDistance = distance;
            }
        }
        return closestIndex;
    }

    public void ShakeScreen()
    {
        StageCamera.Instance.Shake(Vector2.up * 1.5f);
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
