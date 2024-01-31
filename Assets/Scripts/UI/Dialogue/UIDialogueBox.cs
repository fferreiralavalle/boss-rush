using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDialogueBox : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject textDoneIndicator;

    public void Load(TextDialogueData dialogue)
    {
        text.text = dialogue.text;
    }

    public void Open()
    {
        UIMenuAnim anim = GetComponent<UIMenuAnim>();
        if (anim.IsClosed)
        {
            anim.OpenDialog();
        }
    }

    public void Close()
    {
        UIMenuAnim anim = GetComponent<UIMenuAnim>();
        if (!anim.IsClosed)
        {
            GetComponent<UIMenuAnim>().CloseDialog(true);
        }
    }

    public void ShowIndicator()
    {
        textDoneIndicator.SetActive(true);
    }

    public void HideIndicator()
    {
        textDoneIndicator.SetActive(false);
    }
}
