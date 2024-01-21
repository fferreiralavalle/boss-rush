using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MoveController))]
public class FollowMove : MonoBehaviour
{
    public Transform target;
    public bool followPlayerInstead = false;

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
        moveController.MoveTowards(target.position);
    }
}
