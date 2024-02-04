using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GainSpecialbarOnMainDamage : Power
{
    public float gainMultiplier = 1.25f;
    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.playerWeapons.onPrimaryWeaponSpawn += AttatchGainOnEnd;
    }

    public override void HandleRemove()
    {
        _player.playerWeapons.onPrimaryWeaponSpawn -= AttatchGainOnEnd;
        base.HandleRemove();
    }

    public void AttatchGainOnEnd(Projectile projectile)
    {
        projectile.onDealDamage += (DamageSummary sum, float healthleft) => GainExtraEnergy();
    }

    public void GainExtraEnergy()
    {
        float gainAmount = _player.specialChargeGainedPerHit * (gainMultiplier - 1f);
        _player.specialBar.Heal(gainAmount, true);
    }


}
