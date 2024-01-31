using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuMaster : MonoBehaviour
{
    public static UIMenuMaster Instance;

    public Transform menus;

    private void Awake()
    {
        Instance = this;
    }

    public UIMenu OpenMenu(UIMenu menuPrefab)
    {
        if (menuPrefab != null)
        {
            // Disabled previous menu
            if (menus.childCount > 0)
            {
                menus.GetChild(menus.childCount - 1).GetComponent<UIMenu>().Close(true);
            }
            UIMenu newMenu = Instantiate(menuPrefab, menus);
            newMenu.Open();
            GameMaster.Instance.Player?.StartListening();
            return newMenu;
        }
        return null;
    }

    public UIMenu CurrentMenu { get { return menus.childCount > 0 ? menus.GetChild(menus.childCount - 1).GetComponent<UIMenu>() : null; } }
}
