using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float speed = 3f;

    protected float speedMultiplier = 1f;
    protected Vector3 pushForce = Vector2.zero;

    public void OnFixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + pushForce, pushForce.magnitude * Time.fixedDeltaTime);
        pushForce /= 2;
    }

    // Uses own speed
    public void MoveTowards(Vector3 position)
    {
        MoveTowards(position, speed * speedMultiplier);
    }
    // Uses custom speed
    public void MoveTowards(Vector3 position, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.fixedDeltaTime);
    }

    public void Push(Vector3 pushForce)
    {
        this.pushForce += pushForce;
    }

    public float SpeedMutiplier {  get { return speedMultiplier; }  set { speedMultiplier = value; } }

    public void ResetSpeedMultiplier()
    {
        speedMultiplier = 1f;
    }

    public float Speed { get { return speed * speedMultiplier; } set { speed = value; } }
}
