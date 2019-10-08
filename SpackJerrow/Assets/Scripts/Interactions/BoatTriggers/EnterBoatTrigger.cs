using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBoatTrigger : BaseBoatTriggers
{
    public override Action<Sailor, bool> NotifyBoat
    {
        get
        {
            return attachedBoat.SailorBoatStateChanged;
        }
    }

  

}
