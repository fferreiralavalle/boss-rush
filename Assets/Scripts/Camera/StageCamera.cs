using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCamera : MonoBehaviour
{
    public static StageCamera Instance;

    public float shakeTime = .5f;
    public float speed = 5f;

    protected Vector2 shakePower = Vector2.zero;
    protected Vector3 _originalPos;

    protected LeanTween _currentTween;
    protected int direction = 1;
    protected float _timePassed = 0;

    private void Awake()
    {
        if (Instance) Destroy(Instance.gameObject);
        _originalPos = transform.position;
        Instance = this;
    }

    public void Shake(Vector2 shakePower)
    {
        this.shakePower = shakePower;
        _timePassed = 0;
    }

    private void FixedUpdate()
    {
        float speed = shakePower.magnitude * Time.fixedDeltaTime * this.speed;
        if (_timePassed > shakeTime)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                OriginalPosition,
                speed
            );
            return;
        }
        _timePassed += Time.fixedDeltaTime;
        Vector3 target = OriginalPosition + (Vector3)shakePower * direction;
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed
        );
        if (Vector3.Distance(transform.position, target) == 0)
        {
            direction *= -1;
        }
    }


    public Vector3 OriginalPosition { get { return _originalPos; } }

    public Quaternion GetCameraRotation { get { return transform.localRotation; } }

}
