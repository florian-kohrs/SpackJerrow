using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubuSwordFight : PirateSwordFight
{

    public float spinSpeed;

    public bool isSpinning;

    protected override void OnStartAttack(int attackIndex)
    {
        if(attackIndex == 2)
        {
            this.DoDelayed(0.2f, delegate {
                isSpinning = true;
                transform.LookAt(target.transform);
                r.isKinematic = true;
            });
        }
    }

    protected override bool EvaluateAggressionLoss()
    {
        return false;
    }

    protected override bool TriggerEvaluator(GameObject o, PartyAffiliation c)
    {
        return base.TriggerEvaluator(o,c) && GameManager.Story.isFightingBubu;
    }

    protected override void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(new Vector3(0, spinSpeed * Time.deltaTime, 0));
            //Vector3 newEuler = transform.eulerAngles;
            //newEuler.y += spinSpeed * Time.deltaTime;
            //transform.eulerAngles = newEuler;
        }
        else
        {
            base.Update();
        }
    }

    protected override void OnAttackOver(int attackIndex)
    {
        r.isKinematic = false;
        isSpinning = false;
    }

    protected override bool LookAtPlayer()
    {
        return !isSpinning;
    }
    
}
