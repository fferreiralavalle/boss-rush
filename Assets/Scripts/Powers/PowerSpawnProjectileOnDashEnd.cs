using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSpawnProjectileOnDashEnd : Power
{
    public Projectile projectile;
    public override void Initiate(Player player, PowerData powerData)
    {
        base.Initiate(player, powerData);
        player.onDashEnd += HandleDashEnd;
    }

    public void HandleDashEnd()
    {
        Projectile instance = Instantiate(projectile).Initiate(_player);
        instance.transform.position = _player.transform.position;
    }

    private void OnDisable()
    {
        _player.onDashEnd -= HandleDashEnd;
    }
}
