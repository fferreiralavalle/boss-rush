using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpPower : Power
{
    public float speedMultiplier = 1.5f;
    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.moveController.SpeedMutiplier *= speedMultiplier;
    }

    public override void HandleRemove()
    {
        _player.moveController.SpeedMutiplier /= speedMultiplier;
        base.HandleRemove();
    }
}
