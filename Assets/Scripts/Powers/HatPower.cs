using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatPower : Power
{
    public GameObject coolHat;

    protected GameObject hatInstance;

    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        hatInstance = Instantiate(coolHat, player.animator.transform);
        hatInstance.name = "Hat";
        player.animator.Rebind();
    }

    public override void HandleRemove()
    {
        Destroy(hatInstance);
        base.HandleRemove();
    }
}
