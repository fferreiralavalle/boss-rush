using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageManager : MonoBehaviour
{
    public static NextStageManager Instance;
    public List<StageInfo> possibleStages = new List<StageInfo>();

    public StageInfo finalStage;

    protected StageInfo nextStage = null;

    private void Awake()
    {
        Instance = this;
        GenerateNextStage();
    }

    public void GenerateNextStage()
    {
        List<StageInfo> remainingStages = GetRemainingStages();
        if (remainingStages.Count == 0)
            nextStage = finalStage;
        else
        {
            int randomIndex = Random.Range(0, remainingStages.Count);
            nextStage = remainingStages[randomIndex];
        }
    }

    public StageInfo NextStage { get { return nextStage; } }

    public List<StageInfo> GetRemainingStages()
    {
        List<StageInfo> remainingStages = new List<StageInfo>();
        foreach (StageInfo stageInfo in possibleStages)
        {
            if (!stageInfo.IsComplete()) remainingStages.Add(stageInfo);
        }
        return remainingStages;
    }
}
