using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class SpritePositionOrder : MonoBehaviour
{
    public bool updateOnStartOnly = false;

    public float yOffset = 0f;
    protected SpriteRenderer _spriteRenderer;
    protected float updateSecondsInterval = 0.25f;
    protected float timePassed = 0f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _spriteRenderer.sortingOrder = (int)((transform.position.y + yOffset) * -1000);
    }

    void Update()
    {
        if (!updateOnStartOnly)
        {
            timePassed += Time.deltaTime;
            if (timePassed > updateSecondsInterval)
            {
                _spriteRenderer.sortingOrder = (int)((transform.position.y + yOffset) * -1000);
                timePassed = 0f;
            }
        }
    }
}
