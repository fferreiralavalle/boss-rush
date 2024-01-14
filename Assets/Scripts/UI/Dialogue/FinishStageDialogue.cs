using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishStageDialogue : DialogueData
{
    public override void OnEnter()
    {
        StageInfo currentStage = NextStageManager.Instance.NextStage;
        currentStage.Complete();
        NextStageManager.Instance.GenerateNextStage();
        DialogueMaster.Instance.NextDialogue();
        StageManager.Instance.GoToBaseStage();
        GameMaster.Instance.HealPlayer();
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
