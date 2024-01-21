using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsMenu : MonoBehaviour
{
    public UIMenuAnim menu;

    public Slider musicVolume;
    public Slider effectsVolume;

    public void Open()
    {
        menu.gameObject.SetActive(true);
        musicVolume.value = AudioMaster.Instance.musicVolume;
        effectsVolume.value = AudioMaster.Instance.effectsVolume;
    }

    public void Close()
    {
        menu.CloseDialog(true);
    }
}
