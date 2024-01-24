using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToStageDialogue : DialogueData
{
    public Stage stage;
    public UITransition transition;
    public override void OnEnter()
    {
        StageManager.Instance.ChangeStage(stage, transition);
        DialogueMaster.Instance.NextDialogue();
    }
}
