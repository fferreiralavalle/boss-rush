using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Game State/Number Variable")]
public class GameStateVariableDataNumber : GameStateVariableData
{
    public float initialValue;

    public override object GetInitialValue()
    {
        return initialValue;
    }
}
