using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnDamaged(DamageSummary damageTaken, float currentHealth);
public delegate void OnHeal(float amount, float currentHealth);

public class Health : MonoBehaviour
{
    public float maxHealth;
    public HealthType healthType;

    protected float _damageTaken = 0;
    protected float _invulnerableTime = 0f;

    public event OnDamaged onDamage;
    public event OnHeal onHeal;
    public Action onDeath;

    public void Damage(DamageSummary damage)
    {
        if (maxHealth > 0 && _invulnerableTime <= 0 && !IsDead)
        {
            _damageTaken = Mathf.Min(maxHealth, _damageTaken + damage.damage);
            onDamage?.Invoke(damage, CurrentHealth);
            if (IsDead)
            {
                onDeath?.Invoke();
            }
        }
    }

    public void Heal(float amount, bool allowRevive = false)
    {
        if (!IsDead || allowRevive)
        {
            _damageTaken = MathF.Max(0, _damageTaken - amount);
            onHeal?.Invoke(amount, CurrentHealth);
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
    public bool IsDead { get { return CurrentHealth <= 0; } }
}
