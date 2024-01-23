using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueParent : DialogueData
{
    public override void OnEnter()
    {
        base.OnEnter();
        List<DialogueData> list = transform.GetComponentsInChildren<DialogueData>().ToList();
        list.Remove(this);
        DialogueMaster.Instance.InsertDialogue(list);
        DialogueMaster.Instance.NextDialogue();
    }
}
