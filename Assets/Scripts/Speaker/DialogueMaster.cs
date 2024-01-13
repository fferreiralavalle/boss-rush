using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueMaster : MonoBehaviour
{
    public static DialogueMaster Instance { get; private set; }

    public UIDialogueBox dialogueBox;

    protected List<DialogueData> dialogueData = new List<DialogueData>();

    protected int dialogueIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(List<DialogueData> dialogue)
    {
        dialogueIndex = -1;
        dialogueBox.gameObject.SetActive(true);
        dialogueData = new List<DialogueData>(dialogue);
        GameMaster.Instance.Player.StartListening();
        NextDialogue();
    }

    public void NextDialogue()
    {
        if (dialogueIndex >= 0)
        {
            dialogueData[dialogueIndex].OnExit();
        }
        dialogueIndex++;
        if (dialogueIndex < dialogueData.Count)
        {
            dialogueData[dialogueIndex].OnEnter();
        }
        else
        {
            dialogueData = new List<DialogueData> { };
            dialogueBox.Close();
            GameMaster.Instance.Player.StopListening();
        }
    }

    public void LoadDialogue(TextDialogueData dialogue)
    {
        dialogueBox.Load(dialogue);
    }

    public bool IsInDialogue { get { return dialogueIndex < dialogueData.Count; } }
}
