using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float destroyAfter = 3f;
    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }
}
