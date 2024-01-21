using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrbitMove : MonoBehaviour
{
    public float initialAngle = 0f;
    public float angleSpeed = 30f;
    public float centerDistance = 1f;
    public float centerDistanceSpeed = 1f;
    public float maxDistance = 10f;
    public Transform center;

    protected Vector3 originalPos = Vector3.zero;

    private void Awake()
    {
        originalPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 targetPos = center ? center.position : originalPos;
        initialAngle += angleSpeed * Time.fixedDeltaTime;
        centerDistance = Mathf.Min(centerDistance + centerDistanceSpeed * Time.fixedDeltaTime, maxDistance);
        Vector3 direction = new Vector2(Mathf.Cos(initialAngle * Mathf.Deg2Rad), Mathf.Sin(initialAngle * Mathf.Deg2Rad));
        Vector3 orbitPosition = targetPos + direction * centerDistance;
        transform.position = orbitPosition;
    }
}
