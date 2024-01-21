using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnHitEntity(Entity entity);

public class LaserAttack : Projectile
{
    public event OnHitEntity onHitEntity;

    public Transform laserFirePoint;
    public LineRenderer lineRenderer;
    public Vector3 direction;
    public string[] layersToHit = new string[] { "Default" };

    [SerializeField] protected float defDistanceRay = 20f;
    public float warnWidth = 0.1f;
    public float warnTime = 0.5f;
    public float turnOffWidthDecreaseSpeed = 1f;
    protected bool isOff = false;
    protected bool isStarting = true;

    protected float _initialWidth = 1;
    protected float _timePassed = 0;

    private void Awake()
    {
        _initialWidth = lineRenderer.widthMultiplier;
        lineRenderer.widthMultiplier = warnWidth;
    }

    private void Update()
    {
        if (isOff)
        {
            lineRenderer.widthMultiplier = Mathf.MoveTowards(lineRenderer.widthMultiplier, 0, turnOffWidthDecreaseSpeed * Time.deltaTime);
            if (lineRenderer.widthMultiplier == 0)
                Destroy(gameObject);
        }
        else if (isStarting)
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= warnTime)
            {
                lineRenderer.widthMultiplier = Mathf.MoveTowards(lineRenderer.widthMultiplier, _initialWidth, turnOffWidthDecreaseSpeed * Time.deltaTime);
                if (lineRenderer.widthMultiplier == _initialWidth)
                    isStarting = false;
            }
        }
        ShootLaser();
    }

    void ShootLaser()
    {
        LayerMask layer_mask = LayerMask.GetMask(layersToHit);
        if (Physics2D.Raycast(laserFirePoint.position, direction, defDistanceRay, layer_mask))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, direction, defDistanceRay, layer_mask);
            Entity hitEntity = _hit.collider.GetComponent<Entity>();
            bool hitCreator = false;
            if (hitEntity)
            {
                if (hitEntity != _creator)
                {
                    if (CanHit)
                        hitEntity.health.Damage(DamageSummaryMod);
                }
                else
                {
                    hitCreator = true;
                }
            }
            if (!hitCreator)
            {
                Draw2DRay(laserFirePoint.position, _hit.point);
                return;
            }
        }
        Draw2DRay(laserFirePoint.position, laserFirePoint.position + direction * defDistanceRay);
    }

    private void Draw2DRay(Vector3 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }

    public override void HandleRemove()
    {
        isOff = true;
    }

    public bool CanHit { get { return !isStarting && !isOff; } }
}
