using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGroup : Projectile
{
    public List<Projectile> projectileChildren = new List<Projectile>();

    public override Projectile Initiate(Entity creator)
    {
        foreach(Projectile p in projectileChildren)
        {
            if(p != null)
            {
                p.Initiate(creator);
            }
        }
        return base.Initiate(creator);
    }
}
