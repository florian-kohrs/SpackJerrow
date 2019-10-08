using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonShooter : MarksmanAgression<Boat, IMovementPredicter>
{

    [SerializeField]
    public Boat boat;

    public float strength;

    public CannonBallDispenser cannon;

    private IProjectileAimSimulator dispenserInfo;

    private new void Awake()
    {
        base.Awake();   
    }

    private void Start()
    {
        dispenserInfo = cannon as IProjectileAimSimulator;
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (dispenserInfo != null)
        {
            yield return new WaitForSeconds(1);
            dispenserInfo.Fire();
        }
    }
    
    protected override bool TriggerEvaluator(GameObject o, Boat b)
    {
        return b != boat;
    }

    protected override void LooseFocus()
    {
        enabled = false;
    }

    protected override void SwitchedTargetTo(Boat target)
    {
        enabled = true;
    }

    private void Update()
    {
        if (GameManager.AllowMovement && target != null && targetMovement != null && dispenserInfo != null)
        {
            Vector3 targetEuler = ProjectileSniper.TakeAim(targetMovement, dispenserInfo);
            // Debug.Log(targetEuler);

            Vector3 currentEuler = dispenserInfo.RotateTowards(targetEuler, strength);
            targetEuler.z = currentEuler.z;

            float magnitude = Mathf.Round((currentEuler - targetEuler).magnitude) % 360;

            if (magnitude < 1)
            {
                if (dispenserInfo.CanFire)
                {
                    if (GameManager.AllowActions)
                    {
                        dispenserInfo.Fire();
                    }
                }
            }
        }
    }

}
