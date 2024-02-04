using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFireProjectilesAtPosition : EnemyState
{
    public Projectile spawnPrefab;
    public GameObject warningPrefab;
    public int spawnAmount = 5;
    public Vector3 targetPosition;
    public float targetDistance = 5f;
    public float degreesSpawnArc = 120f;
    public float initialDegrees = 0f;
    public float spawnDelay = 0.1f;
    public float duration = 5f;
    public bool rotateToLookAtTarget = true;

    protected Coroutine routine;
    protected GameObject warningInstance;

    public EFireProjectilesAtPosition(Enemy enemy, Projectile spawnPrefab, int spawnAmount, Vector3 targetPosition, float duration) : base(enemy)
    {
        this.spawnPrefab = spawnPrefab;
        this.spawnAmount = spawnAmount;
        this.targetPosition = targetPosition;
        this.duration = duration;
        animatorEventName = "Proyectile";
    }

    public override void Enter()
    {
        routine = _enemy.StartCoroutine(SpawnDelay());
        if (warningInstance)
            warningInstance = Object.Instantiate(warningPrefab);
        base.Enter();
    }

    protected IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        float degreesPerProjectile = degreesSpawnArc / spawnAmount;
        for (int i = 0; i < spawnAmount; i++)
        {
            float angleDegrees = initialDegrees + i * degreesPerProjectile;
            float angleRad = angleDegrees * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f);
            Vector3 positionDiff = direction * targetDistance;
            Projectile projectile = Object.Instantiate(spawnPrefab).Initiate(_enemy);
            projectile.transform.position = targetPosition + positionDiff;
            if (projectile.GetComponent<LineMove>())
                projectile.GetComponent<LineMove>().direction = direction * -1;
            if (rotateToLookAtTarget)
                projectile.transform.Rotate(0, 0, angleDegrees + 180f); // All projectiles point right
        }
    }

    public override void OnUpdate()
    {
        _timePassed += Time.deltaTime;
        base.OnUpdate();
        if (_timePassed > duration)
        {
            HandleFinish();
        }
    }


    public override void Leave()
    {
        _enemy.StopCoroutine(routine);
        if (warningInstance) Object.Destroy(warningInstance);
        base.Leave();
    }
}
