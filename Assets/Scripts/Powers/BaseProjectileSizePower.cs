using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectileSizePower : Power
{
    public float sizeIncreaseMultiplier = 1.3f;
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
        projectile.transform.localScale *= sizeIncreaseMultiplier;
    }
}
