using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Projectile))]
public class SpawnOnDestroyProjectile : MonoBehaviour
{
    public Projectile projectileToSpawn;
    public bool inheritRotation = false;

    protected Projectile _originalProjectile;

    private void Awake()
    {
        _originalProjectile = gameObject.GetComponent<Projectile>();
        _originalProjectile.onDestroy += SpawnProjectile;
    }

    public void SpawnProjectile()
    {
        Projectile p = Instantiate(projectileToSpawn, transform).Initiate(_originalProjectile.Creator);
        p.transform.parent = null;
        if (!inheritRotation)
        {
            p.transform.Rotate(transform.rotation.eulerAngles  * -1);
        }
    }

}
