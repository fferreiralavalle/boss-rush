using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELineProjectiles : EnemyState
{
    public Projectile spawnPrefab;
    public int spawnAmount = 5;
    public Transform spawnPosition;
    public float spawnDistanceFromCenter = 1f;
    public float degreesSpawnArc = 120f;
    public float spawnDelay = 0.1f;
    public float duration = 5f;

    protected Coroutine routine;

    public ELineProjectiles(Enemy enemy, Projectile spawnPrefab, int spawnAmount, Transform spawnPosition, float duration) : base(enemy)
    {
        this.spawnPrefab = spawnPrefab;
        this.spawnAmount = spawnAmount;
        this.spawnPosition = spawnPosition;
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
        Vector3 spawnDirection = (GameMaster.Instance.Player.transform.position - spawnPosition.position).normalized;
        float degreesDiff = Mathf.Atan2(spawnDirection.y, spawnDirection.x);
        for (int i = 0; i < spawnAmount; i++)
        {
            float angle = degreesDiff + (i * degreesPerProjectile - degreesSpawnArc / 2) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
            Vector3 positionDiff = direction * spawnDistanceFromCenter;
            Projectile projectile = Object.Instantiate(spawnPrefab).Initiate(_enemy);
            projectile.transform.position = spawnPosition.position + positionDiff;
            projectile.GetComponent<LineMove>().direction = direction;
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
