using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCircle : MonoBehaviour
{
    public LaserAttack attack;

    public float initialDegrees = -90f;
    public float speed = 10f;

    private void FixedUpdate()
    {
        initialDegrees += speed * Time.fixedDeltaTime;
        Vector2 direction = new Vector2(Mathf.Cos(initialDegrees * Mathf.Deg2Rad), Mathf.Sin(initialDegrees * Mathf.Deg2Rad));
        attack.direction = direction;
    }
}
