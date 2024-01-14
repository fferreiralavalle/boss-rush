using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerChoice : MonoBehaviour
{
    public Image icon;
    public Image equipImage;
    public TextMeshProUGUI powerName;
    public TextMeshProUGUI powerDescription;
    public TextMeshProUGUI cost;

    public Color normalColor;
    public Color selectedColor;

    protected PowerData power;

    public UIPowerChoice Load(PowerData power, bool isEquipped)
    {
        this.power = power;
        if (icon)
            icon.sprite = power.Icon;
        powerName.text = power.Name;
        powerDescription.text = power.Description;
        cost.text = isEquipped ? "Equipped!" : "Equip";

        equipImage.color = isEquipped ? selectedColor : normalColor;

        return this;
    }

    public void HandleEquip()
    {
        bool isEquipped = PowerManager.Instance.IsPowerEquipped(power.Id);
        if (isEquipped)
        {
            PowerManager.Instance.RemovePower(power.Id);
        }
        else
        {
            PowerManager.Instance.AddPower(power.Id);
        }
        Load(power, !isEquipped);
    }
}
