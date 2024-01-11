using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public Stage activeStage;

    private void Awake()
    {
        Instance = this;
    }

    public Stage GetActiveStage()
    {
        return  activeStage;
    }
}
