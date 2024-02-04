using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLongerPower : Power
{
    public float durationMultiplier = .75f;

    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.dashCooldown *= durationMultiplier;
    }

    public void HandleOnDash(PDashState state, Player player)
    {
        /*state.invulnerabilityTime = player.dashInvulnerability * amount;
        state.distance = player.dashDistance * amount;*/
    }

    public override void HandleRemove()
    {
        _player.dashCooldown /= durationMultiplier;
        base.HandleRemove();
    }
}
