using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PartyAffiliation;

public abstract class PartyAggression : TriggerAggression<PartyAffiliation>
{

    public PartyName party;
    
    protected override bool TriggerEvaluator(GameObject o, PartyAffiliation c)
    {
        return c.party != party;
    }

}
