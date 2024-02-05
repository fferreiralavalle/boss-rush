using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIShowTime : MonoBehaviour
{
    public GameStateVariableDataNumber timeVariable;

    public TextMeshProUGUI text;
    public bool updateEveryFrame = false;

    private void OnEnable()
    {
        float timeSeconds = GameMaster.Instance.GetGameStateVariableValue<float>(timeVariable.Id);
        TimeSpan time = TimeSpan.FromSeconds(timeSeconds);
        text.text = time.ToString("hh':'mm':'ss':'fff");
    }

    private void Update()
    {
        if (updateEveryFrame)
        {
            float timeSeconds = GameMaster.Instance.GetGameStateVariableValue<float>(timeVariable.Id);
            TimeSpan time = TimeSpan.FromSeconds(timeSeconds);
            text.text = time.ToString("hh':'mm':'ss':'fff");
        }
    }
}
