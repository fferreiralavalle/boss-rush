using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeart : MonoBehaviour
{
    public Animator animator;

    /// <summary>
    ///  
    /// </summary>
    /// <param name="amount">2 for full, 1 for half heart, 0 for empty</param>
    /// <returns></returns>
    public UIHeart Load(int amount)
    {
        animator.SetInteger("amount", amount);
        return this;
    }

    public void Appear()
    {
        animator.SetBool("visible", true);
    }

    public void Hide()
    {
        animator.SetBool("visible", false);
    }
}
