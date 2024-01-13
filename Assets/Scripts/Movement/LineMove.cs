using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MoveController))]
public class LineMove : MonoBehaviour
{
    public Vector3 direction = Vector3.one;
    public bool moving = true;

    protected MoveController moveController;

    void Awake()
    {
        moveController = GetComponent<MoveController>();
    }

    void FixedUpdate()
    {
        moveController.MoveTowards(transform.position + direction);
    }
}
