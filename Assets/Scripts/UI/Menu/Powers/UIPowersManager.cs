using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPowersManager : MonoBehaviour
{
    public static UIPowersManager Instance;

    public UIPowerChoice powerPrefab;

    public Transform powerList;

    public UIMenuAnim menuAnim;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPowersChoice(List<PowerData> powers)
    {
        menuAnim.gameObject.SetActive(true);
        foreach (Transform oldPower in powerList)
        {
            Destroy(oldPower.gameObject);
        }
        foreach (PowerData power in powers)
        {
            bool isEquipped = PowerManager.Instance.IsPowerEquipped(power.Id);
            Instantiate(powerPrefab, powerList).Load(power, isEquipped);
        }
    }

    public void Close()
    {
        menuAnim.CloseDialog(true);
        DialogueMaster.Instance.NextDialogue();
    }
}
