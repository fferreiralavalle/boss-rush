using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public UIMenuAnim anim;

    public Action onClose;

    public virtual void Open()
    {
        anim.OpenDialog();
    }

    public virtual void Close(bool isTempt = false)
    {
        if (UIMenuMaster.Instance.menus.childCount <= 1 && !DialogueMaster.Instance.IsInDialogue)
        {
            GameMaster.Instance.Player?.StopListening();
        }
        anim.CloseDialog(isTempt);
        onClose?.Invoke();
    }

    public virtual void TryToClose()
    {
        if (IsOpen())
        {
            Close();
        }
    }

    public bool IsOpen()
    {
        UIMenu currentOpen = UIMenuMaster.Instance.CurrentMenu;
        return currentOpen == this;
    }
}
