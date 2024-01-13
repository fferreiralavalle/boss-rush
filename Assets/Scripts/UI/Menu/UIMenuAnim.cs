using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuAnim : MonoBehaviour
{
    public float time = 0.2f;
    public Transform box;
    public CanvasGroup background;
    public bool autoOpenOnEnable = false;
    public bool destroyAfterClose = false;

    protected bool isCloseTemporary = false;

    private void OnEnable()
    {
        if (autoOpenOnEnable)
            OpenDialog();
    }

    public virtual void OpenDialog()
    {
        background.interactable = true;
        background.blocksRaycasts = true;
    }

    public virtual void CloseDialog(bool isTempt = false)
    {
        isCloseTemporary = isTempt;
        background.interactable = false;
        background.blocksRaycasts = false;
    }

    public virtual void OnComplete()
    {
        if (destroyAfterClose && !isCloseTemporary)
        {
            Destroy(gameObject);
        }
        if (isCloseTemporary)
        {
            gameObject.SetActive(false);
        }
    }
}
