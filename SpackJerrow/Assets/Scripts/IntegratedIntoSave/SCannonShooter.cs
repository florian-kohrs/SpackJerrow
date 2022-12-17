using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCannonShooter : SaveableUnityComponent<CannonShooter>
{

    protected override void restoreComponent(CannonShooter component)
    {
        component.boat = b;
        component.cannon = b.GetComponentInChildren<CannonBallDispenser>();
    }

    private Boat b;

    protected override void saveComponent(CannonShooter component, PersistentGameDataController.SaveType saveType)
    {
        b = component.boat;
    }

}
