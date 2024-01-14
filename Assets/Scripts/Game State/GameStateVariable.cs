using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GameStateVariable
{
    public string id;
    public object value;

    public GameStateVariable(string id, object value)
    {
        this.id = id;
        this.value = value;
    }

    public T GetValue<T>()
    {
        return (T)value;
    }
}
