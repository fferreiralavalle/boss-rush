using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectileOnSpecialUsePower : Power
{
    public Projectile projectile;

    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.playerWeapons.onSpecialSpawn += SpawnProjectile;
    }

    public void SpawnProjectile(Projectile projectileParent)
    {
        Projectile instance = Instantiate(projectile, _player.transform).Initiate(projectileParent.Creator);
        if (instance is ProjectileGroup)
        {
            ProjectileGroup group = (ProjectileGroup)instance;
            foreach(Projectile p in group.projectileChildren)
            {

                if (p.GetComponent<OrbitMove>() != null)
                {
                    p.GetComponent<OrbitMove>().center = _player.transform;
                }
            }
        }
        if (instance.GetComponent<OrbitMove>() != null)
        {
            instance.GetComponent<OrbitMove>().center = _player.transform;
        }
    }
}
