using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailTrigger : BaseBoatTriggers
{
    
    public override Action<Sailor, bool> NotifyBoat
    {
        get
        {
            return attachedBoat.PassengerStateChangedOnSettingSails;
        }
    }

}
