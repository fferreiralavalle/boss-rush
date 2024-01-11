using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDamaged(DamageSummary damageTaken, float currentHealth);

public class Health : MonoBehaviour
{
    public float maxHealth;
    public HealthType healthType;

    protected float _damageTaken = 0;
    protected float _invulnerableTime = 0f;

    public event OnDamaged onDamage;

    public void Damage(DamageSummary damage)
    {
        if (maxHealth > 0 && _invulnerableTime <= 0)
        {
            _damageTaken += damage.damage;
            onDamage?.Invoke(damage, CurrentHealth);
        }
    }

    private void Update()
    {
        _invulnerableTime -= Time.deltaTime;
    }

    public float CurrentHealth { get { return Mathf.Max(0, maxHealth - _damageTaken); } }
    public float HealthPercentage { get { return CurrentHealth / maxHealth; } }

    public float MaxHealth { get { return maxHealth; } }
    public float InvulnerableTime {  get { return _invulnerableTime; }  set { _invulnerableTime = Mathf.Max(_invulnerableTime, value); } }
}
