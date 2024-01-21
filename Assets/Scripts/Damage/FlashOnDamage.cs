using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class FlashOnDamage : MonoBehaviour
{
    public Health health;
    public Material flashMaterial;
    public Color hitColor = Color.white;
    public float duration = 0.1f;

    protected SpriteRenderer spriteRenderer;
    protected Material originalMaterial;
    protected Coroutine flashRoutine;

    private void Awake()
    {
        flashMaterial = new Material(flashMaterial);
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private void Start()
    {
        health.onDamage += (summary, health) => HandleHit();
    }

    public void HandleHit()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    protected IEnumerator FlashRoutine()
    {
        flashMaterial.color = hitColor;
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = originalMaterial;

        flashRoutine = null;
    }
}
