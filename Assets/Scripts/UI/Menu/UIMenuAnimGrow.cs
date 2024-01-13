using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIMenuAnimGrow : UIMenuAnim
{
    public Vector3 initialScale = new Vector3 (0f, 0f, 1f);
    public Vector3 finalScale = new Vector3 (1f, 1f, 1f);
    public Vector3 extraBouncyScale = new Vector3 (0.1f, 0.1f, 1f);
    public float bounceTime = 0.1f;


    public override void OpenDialog()
    {
        base.OpenDialog();
        background?.LeanAlpha(1f, time).setEase(LeanTweenType.easeInOutQuad);
        box.localScale = initialScale;
        box.LeanScale(finalScale + extraBouncyScale, time - bounceTime).setOnComplete(BounceBackToFinal).setEase(LeanTweenType.easeInOutQuad);
    }

    public override void CloseDialog(bool isTempt = false)
    {
        base.CloseDialog(isTempt);
        background?.LeanAlpha(0, time);
        box.LeanScale(finalScale + extraBouncyScale, bounceTime).setOnComplete(BounceBackToInitial).setEase(LeanTweenType.easeInOutQuad);
    }

    public void BounceBackToFinal()
    {
        box.LeanScale(finalScale, bounceTime).setEase(LeanTweenType.easeInOutQuad);
    }

    public void BounceBackToInitial()
    {
        box.LeanScale(initialScale, time - bounceTime).setOnComplete(OnComplete).setEase(LeanTweenType.easeInOutQuad);
    }
}
