using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileData projectileData;
    [SerializeField] protected List<HealthType> _targetTypes = new List<HealthType>();

    protected Entity _creator;

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
        if (health != null && _targetTypes.Contains(health.healthType) && _creator != entity)
        {
            health.Damage(DamageSummary);
        }
    }

    public void SwapTargetFor(HealthType swap, HealthType replaceType)
    {
        _targetTypes.Remove(swap);
        _targetTypes.Add(replaceType);
    }

    public DamageSummary DamageSummary { get { return projectileData.damageSummary; } }
}
