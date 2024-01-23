using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitDialogue : DialogueData
{
    public float time = 1f;

    public override void OnEnter()
    {
        base.OnEnter();
        StartCoroutine(GoNext());
    }

    IEnumerator GoNext()
    {
        yield return new WaitForSeconds(time);
        DialogueMaster.Instance.NextDialogue();
    }
}
