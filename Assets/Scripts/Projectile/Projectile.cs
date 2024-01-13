using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;
    public bool canRehitSameEntity = false;
    [SerializeField] protected List<HealthType> _targetTypes = new List<HealthType>();

    protected Entity _creator;
    protected DamageSummary _damageSummary;

    protected List<Entity> _entitiesHit = new List<Entity>();

    public float damageMultiplier = 1f;

    public Projectile Initiate(Entity creator)
    {
        _creator = creator;
        return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity entity = collision.GetComponent<Entity>();
        if (entity == null) return;
        Health health = entity.health;
        if (health != null && _targetTypes.Contains(health.healthType) && _creator != entity && !HasAlreadyHitEntity(entity))
        {
            DamageSummary damageMod = DamageSummaryMod;
            damageMod.damage *= damageMultiplier;
            health.Damage(damageMod);
            _entitiesHit.Add(entity);
        }
    }

    public bool HasAlreadyHitEntity(Entity entity)
    {
        return _entitiesHit.Contains(entity) && canRehitSameEntity;
    }

    public void SwapTargetFor(HealthType swap, HealthType replaceType)
    {
        _targetTypes.Remove(swap);
        _targetTypes.Add(replaceType);
    }

    public DamageSummary DamageSummaryMod {
        get { return (DamageSummary)projectileData.damageSummary.Clone(); }
    }
}
