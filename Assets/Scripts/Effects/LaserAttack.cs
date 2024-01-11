using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    public Transform laserFirePoint;
    public LineRenderer lineRenderer;

    [SerializeField] protected float defDistanceRay = 20f;

    private void Update()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        if (Physics2D.Raycast(transform.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, transform.right);
            Draw2DRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * defDistanceRay);
        }
    }

    private void Draw2DRay(Vector3 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
