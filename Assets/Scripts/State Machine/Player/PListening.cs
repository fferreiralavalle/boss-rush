using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PListening : PState
{
    public PListening(Player player) : base(player)
    {
        animatorEventName = "Idle";
    }
}
