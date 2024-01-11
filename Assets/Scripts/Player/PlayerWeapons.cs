using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public float mainWeapontSpawnRange = 1f;
    public Projectile mainWeaponAttack;


    public void SpawnMainWeaponAttack(Player player, Vector2 direction)
    {
        Vector2 spawnPoint = direction;
        Projectile mainWeapon = Instantiate(mainWeaponAttack);
        mainWeapon.transform.position = (Vector2)player.transform.position + spawnPoint.normalized * mainWeapontSpawnRange;
        float rotation = (Mathf.Atan2(spawnPoint.y, spawnPoint.x) * Mathf.Rad2Deg);
        mainWeapon.transform.Rotate(0,0, rotation);
    }
}
