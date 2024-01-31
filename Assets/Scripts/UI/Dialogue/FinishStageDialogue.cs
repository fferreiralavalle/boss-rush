using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishStageDialogue : DialogueData
{
    public GameStateVariableDataNumber bossesDefeated;
    public override void OnEnter()
    {
        StageInfo currentStage = NextStageManager.Instance.NextStage;
        currentStage.Complete();
        float bossesDefeatedAmount = GameMaster.Instance.GetGameStateVariableValue<float>(bossesDefeated.Id);
        GameMaster.Instance.SetGameStateVariable(bossesDefeated.Id, bossesDefeatedAmount + 1f);
        NextStageManager.Instance.GenerateNextStage();
        DialogueMaster.Instance.NextDialogue();
        StageManager.Instance.GoToBaseStage();
        GameMaster.Instance.ResetPlayBars();
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
