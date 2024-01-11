using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustToCameraRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = StageCamera.Instance.GetCameraRotation;
    }
}
