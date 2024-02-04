using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalLoadDialogue : DialogueData
{
    public List<SpawnCondition> spawnConditions = new List<SpawnCondition>();
    public List<PowerData> powersRequired = new List<PowerData>();

    public List<DialogueData> ifMeetsConditions = new List<DialogueData>();
    public List<DialogueData> ifDoesntMeetConditions = new List<DialogueData>();

    public override void OnEnter()
    {
        base.OnEnter();
        DialogueMaster.Instance.StartDialogue(MeetsConditions() ? ifMeetsConditions : ifDoesntMeetConditions);
    }

    public bool MeetsConditions()
    {
        foreach(var condition in spawnConditions)
        {
            if (!condition.MeetsCondition())
            {
                return false;
            }
        }
        foreach (var condition in powersRequired)
        {
            if (!PowerManager.Instance.IsPowerEquipped(condition.Id))
            {
                return false;
            }
        }
        return true;
    }
}
