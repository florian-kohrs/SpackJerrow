using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileAimDispenser : IRangeWeapon
{
    
    Transform transform { get; }

    Vector3 RotateTowards(Vector3 eulerAngle, float handleSpeed = 1);

}
