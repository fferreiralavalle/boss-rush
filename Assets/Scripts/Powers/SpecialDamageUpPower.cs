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

    public void HandleSpecialSpawn(Projectile special)
    {
        special.damageMultiplier *= damageMultiplier;
    }
}
