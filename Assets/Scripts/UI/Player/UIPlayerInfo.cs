using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerInfo : MonoBehaviour
{
    public static UIPlayerInfo Instance;

    public UIHeart heartPrefab;

    public UIMenuAnim anim;

    public Transform heartsContainer;
    public int healthPerHeart = 2;

    private void Awake()
    {
        Instance = this;
    }

    public void Show()
    {
        anim.gameObject.SetActive(true);
    }

    public void Hide()
    {
        anim.CloseDialog();
    }

    public void UpdateMaxHearts()
    {
        foreach(Transform hearts in heartsContainer)
        {
            Destroy(hearts.gameObject);
        }
        float maxHealth = GameMaster.Instance.Player.health.maxHealth;
        float currentHealth = GameMaster.Instance.Player.health.CurrentHealth;
        for ( int i = 0; i < Mathf.Ceil(maxHealth / healthPerHeart); i++ )
        {
            if (healthPerHeart * i >= currentHealth)
                Instantiate(heartPrefab, heartsContainer).Load(healthPerHeart);
            else
                Instantiate(heartPrefab, heartsContainer).Load((int)currentHealth - healthPerHeart * i);
        }
    }

    public void UpdateHealth()
    {
        Player player = GameMaster.Instance.Player;
        if (player != null)
        {
            float maxHealth = GameMaster.Instance.Player.health.maxHealth;
            float currentHealth = GameMaster.Instance.Player.health.CurrentHealth;
            for (int i = 0; i < Mathf.Ceil(maxHealth / healthPerHeart); i++)
            {
                heartsContainer.GetChild(i).GetComponent<UIHeart>().Load(
                    Mathf.Clamp((int)(currentHealth - healthPerHeart * i), 0, healthPerHeart)
                );
            }
        }
    }
}
