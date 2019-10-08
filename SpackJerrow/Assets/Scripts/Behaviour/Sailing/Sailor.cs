using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sailor : SaveableMonoBehaviour
{

    public abstract void BoatChange(Boat newBoat);

    public Boat boat;

    public virtual void ShipMoving(Vector3 moveDirection) { }

    public abstract void IsInSettingSailAreaChanged(bool isInside);

    public abstract void IsInSteeringAreaChange(bool isInside);

    public void setNewBoat(Boat newBoat)
    {
        boat = newBoat;
        BoatChange(newBoat);
    }

}
