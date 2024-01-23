using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTransitionDialogue : DialogueData
{
    public UITransition transition;
    public bool waitForCoverScreenTime = true;

    public override void OnEnter()
    {
        base.OnEnter();
        TransitionMaster.Instance.PlayTransition(transition);
        if (waitForCoverScreenTime )
        {
            StartCoroutine(WaitForCover());
        }
        else
        {
            DialogueMaster.Instance.NextDialogue();
        }
    }

    IEnumerator WaitForCover()
    {
        yield return new WaitForSeconds(transition.timeToCoverScreen);
        DialogueMaster.Instance.NextDialogue();
    }
}
