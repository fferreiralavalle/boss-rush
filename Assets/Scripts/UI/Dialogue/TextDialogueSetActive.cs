using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDialogueSetActive : DialogueData
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public bool setActiveStateTo = true;

    public override void OnEnter()
    {
        base.OnEnter();
        foreach(GameObject go in gameObjects)
        {
            go.SetActive(setActiveStateTo);
        }
        DialogueMaster.Instance.NextDialogue();
    }
}
