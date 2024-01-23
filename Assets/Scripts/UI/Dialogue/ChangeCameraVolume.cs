using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeCameraVolume : DialogueData
{
    public VolumeProfile profile;

    public override void OnEnter()
    {
        base.OnEnter();
        if (Camera.main.GetComponent<Volume>())
            Camera.main.GetComponent<Volume>().profile = profile;
        DialogueMaster.Instance.NextDialogue();
    }
}
