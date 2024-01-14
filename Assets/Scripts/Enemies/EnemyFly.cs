using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyFly : Enemy
{
    public float timeBetweenAttacks = 2f;

    [Header("Fether Projectile Attack")]
    public Projectile featherProyectilePrefab;
    public Transform featherSpawnPosition;
    public int proyectileAmount = 5;
    public float featherAttackDuration = 2;
    public List<Transform> featherAttackPositions = new List<Transform>();

    [Header("Attack From Sky")]
    public Projectile stompFromSkyPrefab;
    public GameObject previewStompPrefab; 
    public Transform outOfScreenPosition;
    public float timeToReaper = 1f;
    public float flyUpSpeedMultiplier = 1.5f;
    public Vector3 offSkyAttack = Vector3.up * 5;

    [Header("Jump Around")]
    public Projectile stompPrefab;
    public List<Transform> jumpPositions = new List<Transform>();

    protected EIdleState idleState;
    protected EJumpAroundState jumpAroundState;

    // Feather Attack States
    protected ELineProjectiles throwProjectileState;
    protected EMoveToPosition moveToProyectileFirePositionState;

    // Attack from sky Attack States
    protected EMoveToPosition moveToAttackFromSkyPositionState;
    protected EBanishAndReappearState reappearState;

    protected int featherAttacksInARow = 0;

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

        moveToProyectileFirePositionState = new EMoveToPosition(this, featherAttackPositions[0].position);
        throwProjectileState = new ELineProjectiles(this, featherProyectilePrefab, proyectileAmount, featherSpawnPosition, featherAttackDuration);

        jumpAroundState = new EJumpAroundState(this, moveController.speed * 2f, 1f);

        moveToAttackFromSkyPositionState = new EMoveToPosition(this, outOfScreenPosition.position);
        moveToAttackFromSkyPositionState.speedMultiplier = flyUpSpeedMultiplier;
        reappearState = new EBanishAndReappearState(this, Vector2.zero);
        reappearState.timeToReppear = timeToReaper;
        reappearState.speedMultiplier = flyUpSpeedMultiplier;

        jumpAroundState.onReachTargetPrefab = stompPrefab;
        jumpAroundState.onFinish += GoIdle;
        jumpAroundState.AnimatorEventName = "Jump";
        jumpAroundState.animatorStompEventName = "Stomp";

        moveToProyectileFirePositionState.onFinish += FeatherAttack;
        throwProjectileState.onFinish += HandelFeatherAttackEnd;

        moveToAttackFromSkyPositionState.onFinish += FromSkyAttack;
        moveToAttackFromSkyPositionState.AnimatorEventName = "Move";

        reappearState.onAppearPrefab = stompFromSkyPrefab;
        reappearState.targetPreviewPrefab = previewStompPrefab;
        reappearState.AnimatorEventName = "Air Attack";
        reappearState.onFinish += GoIdle;
        reappearState.onLand += ShakeScreen;

        idleState.onFinish += PrepareAttack;

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
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 1:
                MoveToSkyAttackPosition();
                break;
            case 2:
                MoveToFeatherFirePosition();
                break;
            default:
                JumpAround();
                break;
        }
    }

    public void MoveToSkyAttackPosition()
    {
        stateMachine.ChangeState(moveToAttackFromSkyPositionState);
    }

    public void FromSkyAttack()
    {
        reappearState.target = GameMaster.Instance.Player.transform.position;
        reappearState.timeToReppear = health.HealthPercentage < 0.5f ? timeToReaper * 0.5f : timeToReaper;
        stateMachine.ChangeState(reappearState);
    }

    public void MoveToFeatherFirePosition()
    {
        Player player = GameMaster.Instance.Player;
        // Order by lowest distance to player
        List<Transform> positions = featherAttackPositions.OrderBy(pos => Vector3.Distance(pos.position, player.transform.position)).ToList();
        // Remove from posibilities closest to player
        positions.RemoveAt(0);
        int randomPositionIndex = Random.Range(0, positions.Count);
        moveToProyectileFirePositionState.position = positions[randomPositionIndex].position;
        stateMachine.ChangeState(moveToProyectileFirePositionState);
    }

    public void FeatherAttack()
    {
        throwProjectileState.spawnAmount = proyectileAmount + featherAttacksInARow;
        stateMachine.ChangeState(throwProjectileState);
    }

    public void HandelFeatherAttackEnd()
    {
        if (featherAttacksInARow < GetFeatherAttackWavesAmount())
        {
            featherAttacksInARow++;
            MoveToFeatherFirePosition();
        }
        else
        {
            featherAttacksInARow = 0;
            GoIdle();
        }
    }

    public int GetFeatherAttackWavesAmount()
    {
        float healthPercentage = health.HealthPercentage;
        if (healthPercentage < 0.25f)
        {
            return 3;
        }
        else if (healthPercentage < 0.5f)
        {
            return 3;
        }
        return 2;
    }

    public void JumpAround()
    {
        List<Vector3> randomJumpPosition = new List<Vector3>();
        List<int> availableIndex = new List<int>();
        int index = 0;
        jumpPositions
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

    public void ShakeScreen()
    {
        StageCamera.Instance.Shake(Vector2.up);
    }

    public float TimeBetweenAttacks
    {
        get
        {
            if (health.HealthPercentage < 0.5f)
                return timeBetweenAttacks * 0.5f;
            return timeBetweenAttacks;
        }
    }
}
