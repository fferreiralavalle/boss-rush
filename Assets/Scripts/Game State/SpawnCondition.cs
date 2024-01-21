using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SpawnCondition
{
    public float value;
    public SpawnCheckEnum condition;
    public GameStateVariableDataNumber variable;

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
                return value <= currentValue;
            case SpawnCheckEnum.GreaterOrEqual:
                return value >= currentValue;
        }
    }
}
