using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualRotateDispenser : ProjectileDispenser, IProjectileAimSimulator
{

    [Tooltip("How fast can the object be rotated")]
    [Range(0.1f,200f)]
    public float handleDrag = 1;


    public virtual Vector3 RotateTowards(Vector3 to, float handleSpeed = 1)
    {
        Vector3 eulerDelta = to - transform.eulerAngles;
        Vector3 newEulerDelta = (to - transform.eulerAngles).normalized * handleSpeed / handleDrag;
        Vector3.ClampMagnitude(newEulerDelta, eulerDelta.magnitude);
        transform.eulerAngles += newEulerDelta;
        return transform.eulerAngles;
    }
    
}
