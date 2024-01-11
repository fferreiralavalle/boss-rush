using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossInfo : MonoBehaviour
{
    public static BossInfo Instance;

    public Image bossFillbar;
    public Transform healhbarBody;
    public Color healthColor = Color.black;
    public Color damageColor = Color.white;

    public float fillChangeSpeed = 0.03f;
    public float shakeTime = 0.3f;
    public Vector3 shakeIntensity = new Vector3(20f, 20f, 0f);

    protected Enemy activeEnemy;
    protected float targetFillAmount = 1f;
    protected Vector3 barBodyLocalPos;

    private void Awake()
    {
        Instance = this;
        barBodyLocalPos = healhbarBody.localPosition;
    }

    public void Load(Enemy enemy)
    {
        activeEnemy = enemy;
        targetFillAmount = bossFillbar.fillAmount = enemy.health.CurrentHealth / activeEnemy.health.MaxHealth;
        activeEnemy.health.onDamage += UpdateHealth;
    }

    public void UpdateHealth(DamageSummary damage, float currentHealth)
    {
        targetFillAmount = currentHealth / activeEnemy.health.MaxHealth;
        ShakeBar();
    }

    public void ShakeBar()
    {
        LeanTween.moveLocal(healhbarBody.gameObject, barBodyLocalPos + shakeIntensity, shakeTime / 4f).setOnComplete(() =>
        {
            LeanTween.moveLocal(healhbarBody.gameObject, barBodyLocalPos - shakeIntensity, shakeTime / 2f).setOnComplete(() =>
            {
                LeanTween.moveLocal(healhbarBody.gameObject, barBodyLocalPos, shakeTime / 4f);
            });
        });
    }

    public void Update()
    {
        if (targetFillAmount != bossFillbar.fillAmount)
        {
            bossFillbar.fillAmount = Mathf.MoveTowards(bossFillbar.fillAmount, targetFillAmount, fillChangeSpeed);
            bossFillbar.color = damageColor;
        }
        else
        {
            bossFillbar.color = healthColor;
        }
    }
}
