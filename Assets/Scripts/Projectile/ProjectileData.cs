using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Projectile/Basic")]
public class ProjectileData : ScriptableObject
{
    public DamageSummary damageSummary;
    public float duration = 1f;
    public float inmobileTime = 0.8f;
    public AudioData spawnSound;
}
