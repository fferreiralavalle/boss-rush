using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDamageUpPower : Power
{
    public float damageMultiplier = 1.3f;
    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.playerWeapons.onPrimaryWeaponSpawn += HandleIncreaseDamage;
    }

    public override void HandleRemove()
    {
        _player.playerWeapons.onPrimaryWeaponSpawn -= HandleIncreaseDamage;
        base.HandleRemove();
    }

    public void HandleIncreaseDamage(Projectile projectile)
    {
        projectile.damageMultiplier *= damageMultiplier;
    }
}
