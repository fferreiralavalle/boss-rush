using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpawnCondition
{
    public GameStateVariableDataNumber variable;
    public SpawnCheckEnum condition;
    public float value;

    public bool MeetsCondition()
    {
        float currentValue = GameMaster.Instance.GetGameStateVariableValue<float>(variable.id);
        switch(condition)
        {
            default:
                return value == currentValue;
            case SpawnCheckEnum.NotEquals:
                return value != currentValue;
            case SpawnCheckEnum.LessOrEqual:
                return currentValue <= value;
            case SpawnCheckEnum.GreaterOrEqual:
                return currentValue >= value;
        }
    }
}
