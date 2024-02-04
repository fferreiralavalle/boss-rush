using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDamageUpPower : Power
{
    public float damageMultiplier = 1.5f;

    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.playerWeapons.onSpecialSpawn += HandleSpecialSpawn;
    }

    public override void HandleRemove()
    {
        _player.playerWeapons.onSpecialSpawn -= HandleSpecialSpawn;
        base.HandleRemove();
    }

    public void HandleSpecialSpawn(Projectile special)
    {
        special.damageMultiplier *= damageMultiplier;
    }
}
