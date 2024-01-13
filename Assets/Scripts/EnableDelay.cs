using System.Collections;
using UnityEngine;

public class EnableDelay : MonoBehaviour
{
    public MonoBehaviour component;
    public float time = 0.1f;

    Coroutine delay;

    void OnEnable()
    {
        delay = StartCoroutine(EnableAfter(time));
    }

    private void OnDisable()
    {
        StopCoroutine(delay);
        component.enabled = false;
    }

    IEnumerator EnableAfter(float time)
    {
        yield return new WaitForSeconds(time);
        component.enabled = true;
    }
}
