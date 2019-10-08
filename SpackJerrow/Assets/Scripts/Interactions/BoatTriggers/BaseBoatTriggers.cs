using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBoatTriggers : MonoBehaviour
{

    public abstract Action<Sailor, bool> NotifyBoat { get; }

    public Boat attachedBoat;

    public Collider triggerArea;

    private void OnTriggerEnter(Collider other)
    {
        Sailor sailor = other.GetComponent<Sailor>();
        if (sailor != null)
        {
            NotifyBoat(sailor, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Sailor sailor = other.GetComponent<Sailor>();
        if (sailor != null)
        {
            NotifyBoat(sailor, false);
        }
    }

}
