using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowFlexible), typeof(PlayerMovementController))]
public class PlayerSailor : Sailor
{

    public FollowFlexible followBoat;
    public PlayerMovementController movement;

    private void Start()
    {
        if(followBoat == null)
        {
            followBoat = GetComponent<FollowFlexible>();
        }

        if (movement == null)
        {
            movement = GetComponent<PlayerMovementController>();
        }
        displayInfo = true;
    }

    public override void BoatChange(Boat newBoat)
    {
        if(newBoat == null)
        {
            followBoat.StopFollow();
        }
        else
        {
            followBoat.StartFollow(newBoat.transform, true);
        }
    }

    public override void ShipMoving(Vector3 moveDirection)
    {
        followBoat.KeepUp();
        //transform.position += moveDirection;
    }

    private bool canReachSail = false;

    private bool canReachRudder = false;

    private float rudderTurnSpeed = 0.2f;

    [Save]
    private bool displayInfo;

    private void FixedUpdate()
    {
        if (boat != null)
        {
            if (canReachSail)
            {
                if (displayInfo)
                {
                    InterfaceController.DisplayText("Hold \"e\" to set sail, hold \"q\" to lower sail",5);
                    displayInfo = false;
                }
                boat.ChangeSailsFor(Input.GetAxis("SetSail") * Time.deltaTime * 0.4f);
            }
            if (canReachRudder)
            {
                Vector3 toRudderVector = boat.rudder.transform.position - transform.position;
                toRudderVector.y = 0;

                Vector3 moveDirection = movement.LookDiretionInput;
                moveDirection.y = 0;

                float angle = Vector3.Angle(moveDirection, toRudderVector);
                if(angle < 90)
                {
                    float minSpareVelocity = 0.5f;
                    float velocityLossFromAngle = 1 - ((Mathf.InverseLerp(0, 90, angle) * minSpareVelocity));
                    boat.rudder.ChangeRudderRotation(movement.LookDiretionInput.magnitude * -boat.rudder.AngleSignOfTheSailorRelativeToTheRudder(transform.position) * velocityLossFromAngle * rudderTurnSpeed);
                }
            }
        }
    }

    public override void IsInSettingSailAreaChanged(bool isInside)
    {
        canReachSail = isInside;
    }

    public override void IsInSteeringAreaChange(bool isInside)
    {
        canReachRudder = isInside;
    }

}
