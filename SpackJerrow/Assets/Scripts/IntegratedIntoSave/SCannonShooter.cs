using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCannonShooter : SaveableUnityComponent<CannonShooter>
{

    protected override void restoreComponent(CannonShooter component)
    {
        component.boat = b;
        component.cannon = b?.transform
            .GetChild(2)
            .GetChild(1)
            .GetChild(0)
            .GetComponent<CannonBallDispenser>();
    }

    private Boat b;

    protected override void saveComponent(CannonShooter component, PersistentGameDataController.SaveType saveType)
    {
        b = component.boat;
    }

}
