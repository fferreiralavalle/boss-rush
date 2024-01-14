using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameStateVariableData : ScriptableObject
{
    public string id;
    public virtual object GetInitialValue() { return null; }

    public string Id { get { return id; } }
}
