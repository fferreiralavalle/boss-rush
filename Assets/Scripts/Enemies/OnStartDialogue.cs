using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStartDialogue : MonoBehaviour
{
    public List<DialogueData> dialoguesOnEncounter = new List<DialogueData>();

    public void Start()
    {
        DialogueMaster.Instance.StartDialogue(dialoguesOnEncounter);
    }
}
