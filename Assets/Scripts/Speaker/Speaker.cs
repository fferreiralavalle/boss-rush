using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    public Animator animator;

    public List<DialogueData> initialDialogue = new List<DialogueData>();
    public bool autoTalkOnStart = true;

    private void Start()
    {
        if (autoTalkOnStart)
            StartSpeak();
    }

    public void StartSpeak()
    {
        DialogueMaster.Instance.StartDialogue(initialDialogue);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.SpeakerClose = this;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null && player.SpeakerClose == this)
        {
            player.SpeakerClose = null;
        }
    }
}
