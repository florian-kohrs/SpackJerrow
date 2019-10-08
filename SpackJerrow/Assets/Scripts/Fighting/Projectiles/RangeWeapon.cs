using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeWeapon : MonoBehaviour, IRangeWeapon
{
    
    public float fireCooldown = 2;

    public bool CanFire { get; private set; } = true;

    public Vector3 DispenseDirection()
    {
        return transform.forward;
    }

    protected virtual void OnCoolDownReset() { }

    public void Fire()
    {
        CanFire = false;
        FireWeapon();
        OnWeaponFired();
        ResetCooldownDelayed(fireCooldown);
    }

    protected abstract void FireWeapon();

    protected virtual void OnWeaponFired() { }
    
    private IEnumerator ResetCooldownDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        CanFire = true;
        OnCoolDownReset();
    }

}
