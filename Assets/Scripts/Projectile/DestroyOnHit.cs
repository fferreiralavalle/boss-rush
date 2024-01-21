using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Projectile))]
public class DestroyOnHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile projectile = collision.GetComponentInParent<Projectile>();
        if (projectile != null)
        {
            bool isPlayer = projectile.Creator is Player;
            if (isPlayer)
            {
                gameObject.GetComponent<Projectile>().HandleRemove();
            }
        }
    }
}
