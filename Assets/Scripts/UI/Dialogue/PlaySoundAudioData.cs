using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundAudioData : DialogueData
{
    public AudioData audioData;

    public override void OnEnter()
    {
        base.OnEnter();
        AudioMaster.Instance.PlaySoundEffect(audioData);
        DialogueMaster.Instance.NextDialogue();
    }
}
