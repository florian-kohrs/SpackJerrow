using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloating : WaterRigidBody
{

    public Inventory inventory;

    public PlayerExtendedMovement movement;

    public Rigidbody body;

    public Breathing breath;

    private void EnterWater()
    {
        breath.CanBreath = false;
        body.useGravity = false;
        movement.SetMovementSpellList(movement.waterMovement);
    }

    public override void OnWaterUpdated()
    {
    }

    private void LeaveWater()
    {
        body.useGravity = true;
        breath.CanBreath = true;
        movement.SetMovementSpellList(movement.landMovement);
    }

    public override Serializable3DVector Velocity { get => body.velocity; set => body.velocity = value; }

    protected override void BehaviourLoaded()
    {
        if (isInWater)
        {
            EnterWater();
        }
        else
        {
            movement.SetMovementSpellList(movement.landMovement);
        }
    }

    [Save]
    private bool isInWater;

    protected override void ApplyVelocity(Vector3 vel)
    {
        transform.position += vel;
    }

    protected override void ApplyFloatData(WaterFloatInfo info)
    {
        if ((isInWater && info.sizePercentageInWater<0.1f) || (!isInWater && info.sizePercentageInWater < 0.65f))
        {
            if (isInWater)
            {
                isInWater = false;
                LeaveWater();
            }
        }
        else
        {
            if (!isInWater)
            {
                isInWater = true;
                EnterWater();
                if (Velocity.v.magnitude > 1)
                {
                    body.velocity *= 0.2f;
                }
            }
            float yForce = ((1-Density)) * Time.deltaTime;

           body.velocity += new Vector3(0, yForce, 0);
            //Velocity.v += 
            
        }
    }

}
