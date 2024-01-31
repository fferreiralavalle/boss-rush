using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MoveController))]
public class FollowMove : MonoBehaviour
{
    public Transform target;
    public Transform directionFlipper;
    public bool followPlayerInstead = false;
    public Vector3 followOffset;

    protected MoveController moveController;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
    }

    private void Start()
    {
        if (followPlayerInstead)
        {
            target = GameMaster.Instance.Player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (directionFlipper)
        {
            Vector3 direction = (target.position + followOffset - directionFlipper.position).normalized;
            directionFlipper.localScale = new Vector3((Mathf.Abs(directionFlipper.localScale.x) * (direction.x < 0 ? -1 : 1)), directionFlipper.localScale.y, directionFlipper.localScale.z);
        }
        moveController.MoveTowards(target.position + followOffset);
    }
}
