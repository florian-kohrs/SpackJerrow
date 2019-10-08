using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringTrigger : BaseBoatTriggers
{

    public override Action<Sailor, bool> NotifyBoat => attachedBoat.PassengerStateChangedOnSteering;

}
