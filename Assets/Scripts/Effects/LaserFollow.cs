using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFollow : MonoBehaviour
{
    public LaserAttack attack;

    public Transform target;
    public bool followPlayerInstead = false;
    public float speed = 1f;

    private void Start()
    {
        if (followPlayerInstead)
        {
            target = GameMaster.Instance.Player.transform;
        }
    }

    private void FixedUpdate()
    {
        Vector3 direction = (target.position - attack.laserFirePoint.position).normalized;
        attack.direction = Vector3.MoveTowards(attack.direction, direction, direction.magnitude * speed * Time.fixedDeltaTime);
    }
}
