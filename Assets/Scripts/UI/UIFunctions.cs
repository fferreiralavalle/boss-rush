using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    public void RetryBossFight()
    {
        StageManager.Instance.GoToBaseStageFromBlack();
        GameMaster.Instance.ResetPlayBars();
        BossInfo.Instance.Hide();
    }

    public void ResetGame()
    {
        GameMaster.Instance.ResetGameState();
        NextStageManager.Instance.GenerateNextStage();
        StageManager.Instance.GoToBaseStageFromBlack();
        GameMaster.Instance.ResetPlayBars();
        BossInfo.Instance.Hide();
    }
}
