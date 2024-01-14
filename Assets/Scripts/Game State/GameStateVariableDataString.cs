using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Game State/String Variable")]
public class GameStateVariableDataString : GameStateVariableData
{
    public string initialValue;

    public override object GetInitialValue()
    {
        return initialValue;
    }
}
