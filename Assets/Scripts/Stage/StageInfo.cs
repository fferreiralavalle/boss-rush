using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Stage/Info")]
public class StageInfo: ScriptableObject
{
    public Stage stage;
    public UITransition transition;
    public Sprite preview;
    public GameStateVariableDataNumber isCompleteStateVariable;

    public bool IsComplete()
    {
        float isComplete = GameMaster.Instance.GetGameStateVariableValue<float>(isCompleteStateVariable.Id);
        return isComplete != 0f;
    }

    public void Complete()
    {
        GameMaster.Instance.SetGameStateVariable(isCompleteStateVariable.Id, 1f);
    }
}
