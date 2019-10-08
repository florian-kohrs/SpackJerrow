using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallDispenser : ManualRotateDispenser
{
    
    public AudioSource audioSource;

    public AudioClip cannonShootClip;

    private Transform cannonRoot;

    private void Start()
    {
        cannonRoot = transform.parent;
    }

    protected override void OnWeaponFired(GameObject bullet)
    {
        if (cannonShootClip != null && audioSource != null)
        {
            audioSource.clip = cannonShootClip;
            audioSource.Play();
        }
        bullet.GetComponent<CannonBall>().shotPosition = transform.position;
    }

    
    public override Vector3 RotateTowards(Vector3 to, float handleSpeed = 1)
    {
        //to.z = transform.eulerAngles.z;
        //Vector3 newEuler = Vector3.SmoothDamp(
        //    transform.eulerAngles, to, ref velocity, (to - transform.eulerAngles).magnitude / 10 * handleSpeed / handleDrag);
        //transform.eulerAngles = newEuler;
        //return newEuler;

        float newYEuler = RotateYTowards(to.y, handleSpeed);
        cannonRoot.eulerAngles = new Vector3(0, newYEuler, 0);
        transform.eulerAngles = new Vector3(RotateXTowards(to.x, handleSpeed), transform.eulerAngles.y, 0);
        return transform.eulerAngles;
    }
    
    public virtual float RotateXTowards(float xEuler, float handleSpeed = 1)
    {
        float newXEuler = transform.eulerAngles.x;
        float deltaEuler = xEuler - newXEuler;
        if (deltaEuler > 180)
        {
            newXEuler -= 360;
        }
        else if (deltaEuler < -180)
        {
            deltaEuler += 360;
        }
        float maxMagnitudeDelta = deltaEuler;
        float sign = Mathf.Sign(maxMagnitudeDelta);
        float deltaX = sign * Mathf.Min(Mathf.Abs(maxMagnitudeDelta), Time.deltaTime * (handleSpeed / handleDrag));
        newXEuler += deltaX;
        return newXEuler;
    }

    public virtual float RotateYTowards(float yEuler, float handleSpeed = 1)
    {
        float newYEuler = transform.eulerAngles.y;
        float deltaEuler = yEuler - newYEuler;
        if (deltaEuler > 180)
        {
            newYEuler -= 360;
        }
        else if(deltaEuler < -180)
        {
            deltaEuler += 360;
        }
        float maxMagnitudeDelta = deltaEuler;
        float sign = Mathf.Sign(maxMagnitudeDelta);
        float deltaY = sign * Mathf.Min(Mathf.Abs(maxMagnitudeDelta), Time.deltaTime * (handleSpeed / handleDrag));
        newYEuler += deltaY;
        return newYEuler;
    }
    
}
