using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameVariable : DialogueData
{
    public GameStateVariableDataNumber variableData;
    public float value = 1;
    public override void OnEnter()
    {
        if (variableData != null)
        {
            GameMaster.Instance.SetGameStateVariable(variableData.Id, value);
        }
        DialogueMaster.Instance.NextDialogue();
        base.OnEnter();
    }
}
