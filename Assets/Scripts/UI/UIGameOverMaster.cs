using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameOverMaster : MonoBehaviour
{
    public static UIGameOverMaster Instance;

    public UIMenuAnim gameOverScreen;
    public AudioData audioData;

    protected Coroutine closeCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        gameOverScreen.gameObject.SetActive(true);
        AudioMaster.Instance.PlayMusic(audioData);
    }

    public void Hide(float delay = 1f)
    {
        if (closeCoroutine == null)
        {
            closeCoroutine = StartCoroutine(CloseAfter(delay));
        }
    }

    protected IEnumerator CloseAfter(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        gameOverScreen.CloseDialog(true);
    }
}
