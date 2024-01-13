using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDialogueData : DialogueData
{
    [SerializeField][TextArea] public string text;
    [SerializeField] protected Speaker speaker;

    public override void OnEnter()
    {
        DialogueMaster.Instance.LoadDialogue(this);
        if (speaker && speaker.animator != null )
        {
            speaker.animator.SetBool("Talk", true);
        }
    }

    public override void OnExit()
    {
        if (speaker && speaker.animator != null)
        {
            speaker.animator.SetBool("Talk", false);
        }
        base.OnExit();
    }

    public string Text { get { return text; } set { text = value; } }
}
