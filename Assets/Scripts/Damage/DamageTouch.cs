using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTouch
{
    public DamageSummary onHitDamage;
    public Collider2D hitbox;
    public List<Entity> hitEntities = new List<Entity>();

    public DamageTouch(DamageSummary onHitDamage, Collider2D hitbox)
    {
        this.onHitDamage = onHitDamage;
        this.hitbox = hitbox;
    }

    public void AddHitEntity(Entity entity)
    {
        hitEntities.Add(entity);
    }

    public void ResetHitEntity()
    {
        hitEntities.Clear();
    }

    public void SetHitboxActive(bool state)
    {
        hitbox.enabled = state;
    }
}
