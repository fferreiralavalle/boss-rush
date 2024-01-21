using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ELaserProjectileState : EnemyState
{
    public Projectile spawnPrefab;
    public int spawnAmount = 5;
    public Transform spawnPosition;
    public float spawnDistanceFromCenter = 0;
    public float degreesSpawnArc = 360f;
    public float degreesOffsettoPlayer = -30;
    public float spawnDelay = 0.1f;
    public float duration = 5f;

    protected Coroutine routine;

    public ELaserProjectileState(Enemy enemy, Projectile spawnPrefab, int spawnAmount, Transform spawnPosition, float duration) : base(enemy)
    {
        this.spawnPrefab = spawnPrefab;
        this.spawnAmount = spawnAmount;
        this.spawnPosition = spawnPosition;
        this.duration = duration;
        animatorEventName = "Proyectile Laser";
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
        Vector3 spawnDirection = (GameMaster.Instance.Player.transform.position - spawnPosition.position).normalized;
        float degreesDiff = Mathf.Atan2(spawnDirection.y, spawnDirection.x) * Mathf.Rad2Deg;
        for (int i = 0; i < spawnAmount; i++)
        {
            float angle = (i * degreesPerProjectile - degreesSpawnArc / 2);
            Vector3 degrees = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 positionDiff = degrees * spawnDistanceFromCenter;
            Projectile projectile = Object.Instantiate(spawnPrefab).Initiate(_enemy);
            projectile.transform.position = spawnPosition.position + positionDiff;
            projectile.GetComponent<LaserCircle>().initialDegrees = degreesDiff + degreesOffsettoPlayer + angle;
            projectiles.Add(projectile);
        }
    }
    public override void Leave()
    {
        foreach(Projectile p in projectiles)
        {
            p.HandleRemove();
        }
        base.Leave();
    }

    public override void OnUpdate()
    {
        _timePassed += Time.deltaTime;
        if (_timePassed > duration)
        {
            HandleFinish();
        }
        base.OnUpdate();
    }
}
