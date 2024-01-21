using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicDialogueData : DialogueData
{
    public AudioData audioData;

    public override void OnEnter()
    {
        base.OnEnter();
        AudioMaster.Instance.PlayMusic(audioData);
        DialogueMaster.Instance.NextDialogue();
    }
}
