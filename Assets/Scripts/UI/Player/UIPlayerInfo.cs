using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    public static UIPlayerInfo Instance;

    public UIHeart heartPrefab;

    public UIMenuAnim anim;

    public Transform heartsContainer;
    public int healthPerHeart = 2;

    public AudioData gainHeartSound;
    public AudioData loseHeartSound;

    public Image specialBar;
    public Image star;
    public Color normalBarColor;
    public Color fullBarColor;

    protected int _heartsAmountBefore = 0;

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
        float maxHealth = GameMaster.Instance.Player.health.maxHealth;
        int heartsAmount = (int) Mathf.Ceil(maxHealth / healthPerHeart);
        int remainingIndex = 0;
        if (heartsAmount > _heartsAmountBefore && _heartsAmountBefore!=0)
        {
            AudioMaster.Instance.PlaySoundEffect(gainHeartSound);
        }
        else if (heartsAmount < _heartsAmountBefore)
        {
            AudioMaster.Instance.PlaySoundEffect(loseHeartSound);
        }
        foreach (Transform heart in heartsContainer)
        {
            // Hide Extra hearts
            if (remainingIndex >= heartsAmount)
            {
                heart.GetComponent<UIHeart>()?.Hide();
            }
            else
            {
                heart.GetComponent<UIHeart>()?.Appear();
            }
            remainingIndex++;
        }
        // Instantiate missing hearts
        for (int i = remainingIndex; i < heartsAmount; i++)
        {
            UIHeart heartUI = Instantiate(heartPrefab, heartsContainer);
            heartUI.transform.localScale = new Vector3 (i % 2 == 0 ? -1 : 1, 1.0f, 1.0f);
        }
        UpdateHealth();
        _heartsAmountBefore = heartsAmount;
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

    public void UpdateSpecialbar()
    {
        Player player = GameMaster.Instance.Player;
        if (player != null)
        {
            float maxSpecial = GameMaster.Instance.Player.specialBar.maxHealth;
            float currentSpecial = GameMaster.Instance.Player.specialBar.CurrentHealth;
            specialBar.fillAmount = currentSpecial / maxSpecial;
            star.color = specialBar.fillAmount == 1 ? fullBarColor : normalBarColor;
        }
    }
}
