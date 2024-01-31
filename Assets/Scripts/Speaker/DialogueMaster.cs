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
        dialogueData = new List<DialogueData>(dialogue);
        GameMaster.Instance.Player.StartListening();
        NextDialogue();
    }

    public void InsertDialogue(List<DialogueData> dialogue)
    {
        if (dialogueData.Count == 0)
        {
            StartDialogue(dialogue);
        }
        else
        {
            dialogueData.InsertRange(dialogueIndex + 1, dialogue);
        }
    }

    public void NextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex >= 1 && (dialogueIndex - 1) < dialogueData.Count)
        {
            dialogueData[dialogueIndex -1].OnExit();
        }
        if (dialogueIndex < dialogueData.Count)
        {
            dialogueData[dialogueIndex].OnEnter();
        }
        else
        {
            dialogueData = new List<DialogueData> { };
            GameMaster.Instance.Player.StopListening();
            dialogueBox?.Close();
        }
    }

    public void CloseDialogueText()
    {
        dialogueBox?.Close();
    }

    public DialogueData CurrentDialogue
    {
        get { return dialogueIndex < dialogueData.Count ? dialogueData[dialogueIndex] : null; }
    }

    public void LoadDialogue(TextDialogueData dialogue)
    {
        if (dialogueBox.gameObject.activeSelf)
            dialogueBox.Open();
        else
            dialogueBox.gameObject.SetActive(true);
        dialogueBox.Load(dialogue);
    }

    public bool IsInDialogue { get { return dialogueIndex < dialogueData.Count; } }
}
