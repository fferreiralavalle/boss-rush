using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveController : MonoBehaviour
{
    public float speed = 3f;

    protected Vector2 targetPosition = Vector2.zero;
    protected Vector2 pushForce = Vector2.zero;

    public void OnFixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + pushForce, pushForce.magnitude * Time.fixedDeltaTime);
        pushForce /= 2;
    }

    // Uses own speed
    public void MoveTowards(Vector2 position)
    {
        MoveTowards(position, speed);
    }
    // Uses custom speed
    public void MoveTowards(Vector2 position, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.fixedDeltaTime);
    }

    public void Push(Vector2 pushForce)
    {
        this.pushForce += pushForce;
    }
}
