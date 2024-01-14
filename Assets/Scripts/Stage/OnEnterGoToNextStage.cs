using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterGoToNextStage : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            StageInfo stageInfo = NextStageManager.Instance.NextStage;
            if (stageInfo != null)
            {
                StageManager.Instance.ChangeStage(stageInfo.stage, stageInfo.transition);
            }
        }
    }
}
