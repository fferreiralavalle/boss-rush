using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnPowerObtain(Power power);

public class PowerManager : MonoBehaviour
{
    public static PowerManager Instance;

    public List<Power> powerDataList = new List<Power>();

    public List<Power> initiallyEquippedPowers = new List<Power>();

    protected Dictionary<string, Power> activePowers = new Dictionary<string, Power>();

    // Events
    public event OnPowerObtain onPowerObtain;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach(Power power in initiallyEquippedPowers)
        {
            AddPower(power.powerData.Id);
        }
    }

    public void AddPower(string powerId)
    {
        Power power = GetPower(powerId);
        Player player = GameMaster.Instance.Player;
        if (power != null && player && !activePowers.ContainsKey(powerId))
        {
            Power powerInstance = Instantiate(power, transform);
            if (powerInstance)
            {
                powerInstance.Initiate(player, power.powerData);
                activePowers.Add(powerId, powerInstance);
                onPowerObtain?.Invoke(powerInstance);
            }
        }
    }

    public void RemovePower(string power)
    {
        if (activePowers.ContainsKey(power))
        {
            Power powerInstance = activePowers[power];
            powerInstance.HandleRemove();
            activePowers.Remove(power);
        }
    }

    public Power GetPower(string power)
    {
        foreach(Power powerData in powerDataList)
        {
            if (powerData.powerData.Id == power)
            {
                return powerData;
            }
        }
        return null;
    }

    public float GetPowersTotalHealthCost()
    {
        float totalHealthCost = 0;
        foreach(Power powerData in activePowers.Values)
        {
            totalHealthCost += powerData.powerData.HealthCost;
        }
        return totalHealthCost;
    }
}
