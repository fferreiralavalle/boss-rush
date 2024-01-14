using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransition : MonoBehaviour
{
    public float duration = 1f;
    public float timeToCoverScreen = 0.2f;

    public void Start()
    {
        Play();
    }

    public void Play()
    {
        Destroy(gameObject, duration);
    }
}
