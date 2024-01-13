using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuAnimMoveIn : UIMenuAnim
{
    public Vector2 screenSide = Vector2.left;

    protected Vector3 _originalPosition = Vector3.zero;

    private void Awake()
    {
        _originalPosition = box.transform.localPosition;
    }
    public override void OpenDialog()
    {
        base.OpenDialog();
        background.LeanAlpha(1f, time).setEase(LeanTweenType.easeInOutQuad);
        box.localPosition = GetStartingPosition();
        box.LeanMoveLocal(_originalPosition, time).setEase(LeanTweenType.easeInOutQuad);
    }

    public override void CloseDialog(bool isTempt = false)
    {
        base.CloseDialog(isTempt);
        background.LeanAlpha(0, time);
        box.LeanMoveLocal(GetStartingPosition(), time).setOnComplete(OnComplete).setEase(LeanTweenType.easeInOutQuad);
    }

    public Vector3 GetStartingPosition()
    {
        return new Vector3(screenSide.x * Screen.width, screenSide.y * Screen.height, 0f);
    }
}
