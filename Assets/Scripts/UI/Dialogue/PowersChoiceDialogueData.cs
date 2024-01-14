using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersChoiceDialogueData : DialogueData
{
    public List<PowerData> powers = new List<PowerData>();

    public override void OnEnter()
    {
        base.OnEnter();
        UIPowersManager.Instance.ShowPowersChoice(powers);
    }
}
