using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Power")]
public class PowerData: ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected string powerName;
    [SerializeField] [TextArea] protected string description;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected float healthCost;

    public string Id { get { return id; } }
    public string Name { get { return powerName; } }
    public string Description { get { return description; } }
    public Sprite Icon { get { return icon; } }
    public float HealthCost { get { return healthCost; } }
}
