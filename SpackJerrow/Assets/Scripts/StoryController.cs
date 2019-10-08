using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryController : SaveableMonoBehaviour
{

    protected override void BehaviourLoaded()
    {
        base.BehaviourLoaded();
    }

    [Save]
    public bool defeatedSteersman;

    [Save]
    public bool isFightingSteersman;

    public bool IsFightingSteersman
    {
        get
        {
            return isFightingSteersman; 
        }
        set
        {
            isFightingSteersman = value;
        }
    }

    [Save]
    public bool isFightingBubu;

}
