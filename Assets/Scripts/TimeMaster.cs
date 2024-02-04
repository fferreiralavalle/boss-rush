using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMaster : MonoBehaviour
{
    public static TimeMaster Instance;

    public GameStateVariableData saveToTime;

    protected bool runTime = false;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTimer()
    {
        runTime = true;
    }

    public void StopTimer()
    {
        runTime = false;
    }

    private void Update()
    {
        if (runTime)
        {
            float timePassed = GameMaster.Instance.GetGameStateVariableValue<float>(saveToTime.Id) + Time.deltaTime; 
            GameMaster.Instance.SetGameStateVariable(saveToTime.Id, timePassed);
        }
    }
}
