using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public float mainWeapontSpawnRange = 1f;
    public float mainWeaponSpawnDelay = 0.1f;
    public Projectile mainWeaponAttack;
    public Transform mainWeaponOrigin;
    public bool spawnAsChildren = true;

    public float specialWeaponSpawnDelay = 0.1f;
    public Vector2 normalizedMousePointCorrection = new Vector2(0, -0.1f);
    public Projectile specialWeaponAttack;


    public event OnProjectileSpawn onPrimaryWeaponSpawn;
    public event OnProjectileSpawn onSpecialSpawn;

    public void SpawnMainWeaponAttack(Player player, Vector2 direction)
    {
        StartCoroutine(SpawnDelay(player, direction));
    }

    public IEnumerator SpawnDelay(Player player, Vector2 direction)
    {
        yield return new WaitForSeconds(mainWeaponSpawnDelay);
        Vector2 spawnPoint = direction;
        Projectile mainWeapon = Instantiate(mainWeaponAttack, spawnAsChildren ? mainWeaponOrigin : null).Initiate(player);
        mainWeapon.transform.position = (Vector2)mainWeaponOrigin.position + spawnPoint.normalized * mainWeapontSpawnRange;
        float rotation = (Mathf.Atan2(spawnPoint.y, spawnPoint.x) * Mathf.Rad2Deg);
        mainWeapon.transform.Rotate(0, 0, rotation);
        onPrimaryWeaponSpawn?.Invoke(mainWeapon);
        mainWeapon.onDealDamage += (damage, enemyHealth)=> OnDealDamage(damage, enemyHealth, player);
    }

    public void OnDealDamage(DamageSummary damage, float enemyHealth, Player player)
    {
        player.specialBar.Heal(player.specialChargeGainedPerHit, true);
    }

    public void SpawnSpecialWeaponAttack(Player player, Vector2 direction)
    {
        player.specialBar.Damage(new DamageSummary(999));
        StartCoroutine(SpawnDelaySpecial(player, direction));
    }

    public IEnumerator SpawnDelaySpecial(Player player, Vector2 direction)
    {
        yield return new WaitForSeconds(mainWeaponSpawnDelay);
        Vector2 spawnPoint = direction;
        Projectile specialAttack = Instantiate(specialWeaponAttack).Initiate(player);
        specialAttack.transform.position = (Vector2)mainWeaponOrigin.position + (spawnPoint.normalized + normalizedMousePointCorrection) * mainWeapontSpawnRange;
        float rotation = (Mathf.Atan2(spawnPoint.y, spawnPoint.x) * Mathf.Rad2Deg);
        specialAttack.transform.Rotate(0, 0, rotation);
        if (specialAttack.GetComponent<LineMove>())
            specialAttack.GetComponent<LineMove>().direction = direction.normalized + normalizedMousePointCorrection;
        onSpecialSpawn?.Invoke(specialAttack);
    }

    private void OnDestroy()
    {
        onPrimaryWeaponSpawn = null;
    }
}
