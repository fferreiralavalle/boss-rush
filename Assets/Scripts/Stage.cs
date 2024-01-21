using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Collider2D stageDimensions;
    public Transform playerSpawn;
    public bool showPlayerGUI = true;
    public AudioData onEnterAudio;


    public Vector2 GetSize()
    {
        return stageDimensions.bounds.size;
    }

    public Vector3 GetCenter()
    {
        return stageDimensions.bounds.center;
    }


}
