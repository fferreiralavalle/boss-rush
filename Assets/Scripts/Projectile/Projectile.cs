using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;
    public bool canRehitSameEntity = false;
    [SerializeField] protected List<HealthType> _targetTypes = new List<HealthType>();
    public bool dontAutodestroy = false;
    public GameObject onDestroySpawn;
    public GameObject onHitSpawn;

    protected Entity _creator;
    protected DamageSummary _damageSummary;

    protected List<Entity> _entitiesHit = new List<Entity>();

    public float damageMultiplier = 1f;
    public OnDamaged onDealDamage;
    public Action onDestroy;

    protected bool destroyed = false;

    public virtual void OnEnable()
    {
        if (!dontAutodestroy)
            StartCoroutine(DestroyAfterTime());
        AudioMaster.Instance?.PlaySoundEffect(projectileData.spawnSound);
    }

    protected IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(projectileData.duration);
        HandleRemove();
    }

    public virtual Projectile Initiate(Entity creator)
    {
        _creator = creator;
        if (_creator.health)
        {
            _creator.health.onDeath += HandleRemove;
        }
        GameMaster.Instance.Player.health.onDeath += HandleRemove;
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
            onDealDamage?.Invoke(damageMod, health.CurrentHealth);
            _entitiesHit.Add(entity);
            if (onHitSpawn)
            {
                GameObject hitInstance = Instantiate(onHitSpawn);
                hitInstance.transform.position = (transform.position + entity.transform.position) / 2f;
            }
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

    public virtual void HandleRemove()
    {
        if (!destroyed)
        {
            if (onDestroySpawn)
            {
                GameObject onDestroyInstance = Instantiate(onDestroySpawn);
                onDestroyInstance.transform.position = transform.position;
            }
            onDestroy?.Invoke();
            Destroy(gameObject);
            destroyed = true;
        }
    }

    public DamageSummary DamageSummaryMod {
        get { return (DamageSummary)projectileData.damageSummary.Clone(); }
    }
    public Entity Creator { get { return _creator; } }
}
