using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    public PowerData powerData;
    protected Player _player;

    public virtual void Initiate(Player player, PowerData powerData)
    {
        _player = player;
        this.powerData = powerData;
    }

    public virtual void HandleRemove()
    {
        Destroy(gameObject);
    }
}
