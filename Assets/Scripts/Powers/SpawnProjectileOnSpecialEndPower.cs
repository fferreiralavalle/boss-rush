using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectileOnSpecialEndPower : Power
{
    public Projectile projectile;

    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.playerWeapons.onSpecialSpawn += AtatchSpawnOnEnd;
    }

    public override void HandleRemove()
    {
        _player.playerWeapons.onSpecialSpawn -= AtatchSpawnOnEnd;
        base.HandleRemove();
    }

    public void AtatchSpawnOnEnd(Projectile projectile)
    {
        projectile.onDealDamage += (DamageSummary sum, float healthleft) => SpawnProjectile(projectile);
    }

    public void SpawnProjectile(Projectile projectileParent)
    {
        Projectile instance = Instantiate(projectile).Initiate(projectileParent.Creator);
        instance.transform.position = projectileParent.transform.position;
    }
}
