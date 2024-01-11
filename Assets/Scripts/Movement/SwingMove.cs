using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMove : MonoBehaviour
{
    public float duration = 0.25f;
    public float swingAmplitudeDegrees = 90f;
    public bool destroyAfter = true;

    protected float targetDegrees;

    void Start()
    {
        targetDegrees = transform.rotation.eulerAngles.z + swingAmplitudeDegrees / 2f;
        transform.Rotate(0, 0, -swingAmplitudeDegrees / 2);
        transform.LeanRotateZ(targetDegrees, duration).setOnComplete(HandleDestroy);
    }

    public void HandleDestroy()
    {
        Destroy(gameObject);
    }

}
