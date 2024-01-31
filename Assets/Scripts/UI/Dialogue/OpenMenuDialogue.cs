using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenuDialogue : DialogueData
{
    public UIMenu menuPrefab;
    public override void OnEnter()
    {
        base.OnEnter();
        UIMenu instance = UIMenuMaster.Instance.OpenMenu(menuPrefab);
        if (instance != null)
        {
            instance.onClose += NextDialogueOnClose;
        }
    }

    public void NextDialogueOnClose()
    {
        DialogueMaster.Instance.NextDialogue();
    }
}
