using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRangeWeapon
{

    bool CanFire { get; }

    void Fire();

    Vector3 DispenseDirection();

}
