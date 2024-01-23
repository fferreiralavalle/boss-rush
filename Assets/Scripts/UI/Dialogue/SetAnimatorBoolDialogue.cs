using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorBoolDialogue : DialogueData
{
    public Animator animator;
    public string variableName;
    public bool setTo = true;

    public override void OnEnter()
    {
        base.OnEnter();
        if (animator)
        {
            animator.SetBool(variableName, setTo);
        }
        DialogueMaster.Instance.NextDialogue();
    }
}
