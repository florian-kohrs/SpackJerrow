using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwordFight : PartyAggression
{
    
    public Collider swordHitBox;

    public Animation defaultSwordHit;

    public Vector3 idleFightPosition;
    public Vector3 idleFightRotation;

    public Vector3 defaultLocalPosition;
    public Vector3 defaulLocalEuler;

    private void Update()
    {
        if(target != null)
        {

        }
    }

}
