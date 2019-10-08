using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PirateSwordFight : PartyMeleeFighter
{

    public Rigidbody r;

    private void Start()
    {
        if (GameManager.Story.IsFightingSteersman && !GameManager.Story.defeatedSteersman)
        {
            r.isKinematic = true;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (target != null && LookAtPlayer())
        {
            transform.LookAt(target.transform);
        }
    }

    protected virtual bool LookAtPlayer()
    {
        return true;
    }

    protected override void MoveToTarget(Vector3 direction)
    {
        r.AddForce(direction.normalized * Time.deltaTime * moveSpeed);
    }

    protected override bool TriggerEvaluator(GameObject o, PartyAffiliation c)
    {
        return base.TriggerEvaluator(o, c) && c.party != PartyAffiliation.PartyName.Unkown;
    }

    protected override void LooseFocus()
    {
        base.LooseFocus();
        r.isKinematic = true;
    }

    protected override void StartFocus(PartyAffiliation target)
    {
        if (enabled)
        {
            base.StartFocus(target);
            r.isKinematic = false;
            HealthController s = GetComponent<HealthController>();
            if (s != null)
            {
                s.enabled = true;
            }
        }
    }

    public void AttackTarget(PartyAffiliation p)
    {
        Focus(p);
    }

}
