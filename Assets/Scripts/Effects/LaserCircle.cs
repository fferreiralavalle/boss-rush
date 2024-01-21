using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCircle : MonoBehaviour
{
    public LaserAttack attack;

    public float initialDegrees = -90f;
    public float speed = 10f;
    public float distanceFromCenter = 1f;

    private void FixedUpdate()
    {
        initialDegrees += speed * Time.fixedDeltaTime;
        Vector3 direction = new Vector3(Mathf.Cos(initialDegrees * Mathf.Deg2Rad), Mathf.Sin(initialDegrees * Mathf.Deg2Rad));
        attack.direction = direction;
        attack.laserFirePoint.position = attack.transform.position + direction * distanceFromCenter;
    }
}
