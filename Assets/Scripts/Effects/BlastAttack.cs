using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class BlastAttack : Projectile
{
    public float maxRadius = 10f;
    public float speed = 1f;
    public float startWidth = 3f;

    protected LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Blast());
    }


    protected IEnumerator Blast()
    {
        float currentRadius = 0f;

        while (currentRadius < maxRadius && !destroyed) {
            currentRadius = Mathf.MoveTowards(currentRadius, maxRadius, Time.deltaTime * speed);
            Draw(currentRadius);
            Damage(currentRadius);
            yield return null;
        }
        HandleRemove();
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / (lineRenderer.positionCount - 1);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }

    protected void Damage(float radius)
    {
        Collider2D[] hittingObjects = Physics2D.OverlapCircleAll(transform.position, radius);
        Collider2D[] safeInnerCircle = Physics2D.OverlapCircleAll(transform.position, radius - lineRenderer.widthMultiplier);
        for (int i = 0;i < hittingObjects.Length; i++)
        {
            if (!safeInnerCircle.Contains(hittingObjects[i]))
            {
                Entity entity = hittingObjects[i].GetComponent<Entity>();
                if (entity == null || _entitiesHit.Contains(entity)) continue;
                Health health = entity.health;
                if (health && _targetTypes.Contains(health.healthType))
                {
                    health.Damage(DamageSummaryMod);
                    _entitiesHit.Add(entity);
                }
            }
        }
    }
}
