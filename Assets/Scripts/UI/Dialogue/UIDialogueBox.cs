using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogueBox : MonoBehaviour
{
    public TextMeshProUGUI text;

    public void Load(TextDialogueData dialogue)
    {
        text.text = dialogue.text;
    }

    public void Close()
    {
        GetComponent<UIMenuAnim>().CloseDialog(true);
    }
}
