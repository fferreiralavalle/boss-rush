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
    public float lineHitboxMultiplier = 0.5f;
    public string[] layersToHit = new string[] { "Default" };

    [SerializeField] protected float defDistanceRay = 20f;

    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        LayerMask layer_mask = LayerMask.GetMask(layersToHit);
        float angle = Mathf.Atan2(direction.y, direction.x);
        Vector2 size = new Vector2(defDistanceRay / 2, lineRenderer.widthMultiplier * lineHitboxMultiplier);
        Vector2 finalOrigin = (Vector2)laserFirePoint.position + size / 2f;
        // if (Physics2D.CapsuleCast(finalOrigin, size, CapsuleDirection2D.Horizontal, angle, direction, defDistanceRay, layer_mask))
        if (Physics2D.Raycast(laserFirePoint.position, direction, defDistanceRay, layer_mask))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, direction, defDistanceRay, layer_mask);
            Entity hitEntity = _hit.collider.GetComponent<Entity>();
            bool hitCreator = false;
            if (hitEntity)
            {
                if (hitEntity != _creator)
                {
                    hitEntity.health.Damage(DamageSummary);
                }
                else
                {
                    hitCreator = true;
                }
            }
            if (!hitCreator)
                Draw2DRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.position + direction * defDistanceRay);
        }
    }

    private void Draw2DRay(Vector3 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
