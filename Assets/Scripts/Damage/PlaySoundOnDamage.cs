using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnDamage : MonoBehaviour
{
    public Health health;
    public AudioData sound;

    private void Start()
    {
        health.onDamage += (summary, health) => HandleDamage();
    }

    protected void HandleDamage()
    {
        AudioMaster.Instance.PlaySoundEffect(sound);
    }
}
