using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOrbitProjectile : EnemyState
{
    public Projectile spawnPrefab;
    public int spawnAmount = 5;
    public Transform orbitPosition;
    public float spawnDistanceFromCenter = 0.1f;
    public float maxDistanceMod = 0f;
    public float degreesSpawnArc = 360f;
    public float spawnDelay = 0.1f;
    public float duration = 7f;

    protected Coroutine routine;

    public EOrbitProjectile(Enemy enemy, Projectile spawnPrefab, int spawnAmount, Transform orbitPosition, float duration) : base(enemy)
    {
        this.spawnPrefab = spawnPrefab;
        this.spawnAmount = spawnAmount;
        this.orbitPosition = orbitPosition;
        this.duration = duration;
        animatorEventName = "Proyectile";
    }

    public override void Enter()
    {
        routine = _enemy.StartCoroutine(SpawnDelay());
        base.Enter();
    }

    protected IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(spawnDelay);
        float degreesPerProjectile = degreesSpawnArc / spawnAmount;
        Vector3 spawnDirection = (GameMaster.Instance.Player.transform.position - orbitPosition.position).normalized;
        float degreesDiff = Mathf.Atan2(spawnDirection.y, spawnDirection.x);
        for (int i = 0; i < spawnAmount; i++)
        {
            float angle = degreesDiff + (i * degreesPerProjectile - degreesSpawnArc / 2);
            Projectile projectile = Object.Instantiate(spawnPrefab, orbitPosition).Initiate(_enemy);
            projectile.transform.parent = null;
            OrbitMove orbitMove = projectile.GetComponent<OrbitMove>();
            if (orbitMove)
            {
                orbitMove.initialAngle = angle;
                orbitMove.center = orbitPosition;
                orbitMove.centerDistance = spawnDistanceFromCenter;
                orbitMove.maxDistance += maxDistanceMod;
            }
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
        base.Leave();
    }
}
