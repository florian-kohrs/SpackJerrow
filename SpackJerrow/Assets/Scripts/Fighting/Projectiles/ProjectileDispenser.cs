using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDispenser : RangeWeapon, IProjectileSimulator
{
    
    public GameObject originalBullet;

    public Transform bulletSpawnPosition;

    [Range(1,1000)]
    public float projectilePower = 100;

    [Range(0.1f,100)]
    public float projectileMass = 1;

    public float ProjectileSpeed => projectilePower / projectileMass;

    public Vector3 SpawnGlobalPosition => bulletSpawnPosition.position;

    private GameObject projectile;

    protected override void FireWeapon()
    {
        projectile = GetProjectile();
        AddForceToProjectile(projectile);
    }
    protected sealed override void OnWeaponFired()
    {
        OnWeaponFired(projectile);
        projectile = null;
    }

    protected virtual void OnWeaponFired(GameObject bullet) { }

    public virtual void AddForceToProjectile(GameObject projectile)
    {
        Rigidbody r = projectile.GetOrAddComponent<Rigidbody>(
            c =>
            { 
                c.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        );
        r.mass = projectileMass;
        r.AddForce(transform.forward * projectilePower, ForceMode.Impulse);
        Destroy(projectile, 15);
    }

    protected virtual GameObject GetProjectile()
    {
        return Instantiate(originalBullet,
            bulletSpawnPosition.position, Quaternion.identity);
    }

}
