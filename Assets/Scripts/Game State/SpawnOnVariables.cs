using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnVariables : MonoBehaviour
{
    public List<SpawnCondition> spawnConditions = new List<SpawnCondition>();
    public GameObject prefab;

    void Start()
    {
        if (MeetsCondition())
        {
            Instantiate(prefab, transform);
        }
    }

    public bool MeetsCondition()
    {
        foreach(var condition in spawnConditions)
        {
            if (!condition.MeetsCondition()) return false;
        }
        return true;
    }
}
