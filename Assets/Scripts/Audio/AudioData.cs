using System;
using UnityEngine;
[Serializable]
public class AudioData
{
    public AudioClip audioClip;
    public float volumeMultiplier = 1.0f;
    [SerializeField] protected float minPitch = 1f;
    [SerializeField] protected float extraPitchRange = 0f;

    public float delay = 0f;
    public bool loop = true;

    public float GetRandomPitch()
    {
        float random = UnityEngine.Random.Range(minPitch, minPitch + extraPitchRange);
        return random;
    }
}
