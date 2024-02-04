using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimerDialogue : DialogueData
{
    public bool stopInstead = false;

    public override void OnEnter()
    {
        if (stopInstead)
            TimeMaster.Instance.StopTimer();
        else
            TimeMaster.Instance.StartTimer();
        DialogueMaster.Instance.NextDialogue();
        base.OnEnter();
    }
}
