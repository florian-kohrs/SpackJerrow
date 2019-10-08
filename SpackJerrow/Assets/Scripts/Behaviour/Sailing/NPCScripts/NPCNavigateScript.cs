using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNavigateScript : Sailor
{

    [Tooltip("0 means cant sail at all, 1 is perfect")]
    [Range(0, 1)]
    public float sailSkill = 0.8f;

    public Transform targetPos;

    protected override void OnFirstTimeBehaviourAwakend()
    {
        if (targetPos != null)
        {
            target = new Serializable3DVector(targetPos.position);
        }
    }

    protected override void setDataBeforeSaving(PersistentGameDataController.SaveType saveType)
    {
        if (targetPos != null)
        {
            target = new Serializable3DVector(targetPos.position);
        }
    }

    //protected override void OnBehaviourConstructed()
    //{
    //    if (target != null)
    //    {
    //        position = new Serializable3DVector(target.position);
    //    }
    //}

    private void Update()
    {
        transform.LookAt(GameManager.Player.transform);
    }

    [Save]
    protected Serializable3DVector target;

    public float setSailSpeed;

    public float movementSpeedOnBoat;

    private bool isBusy = false;

    private void FixedUpdate()
    {
        if (target != null && boat != null)
        {
            Vector3 distance = target.v - boat.transform.position;
            distance.y = 0;

            Vector3 toSailDirection = boat.sailInteractTrigger.ClosestPoint(transform.position) - transform.position;
            toSailDirection.y = 0;

            float travelTimeUntilReachingSail = ((toSailDirection.magnitude / movementSpeedOnBoat) * boat.Velocity.v).magnitude;
            float timeToReduceSailToZero = boat.SailUpTransition / SailsSettingSpeed;
            float averageSailHeightWhileReducing = boat.SailUpTransition / 2;

            Vector3 extendedSailDirection = boat.Velocity.v;

            Vector3 travelDirection = boat.transform.forward;
            travelDirection.y = 0;

            Vector3 turnDirection = travelDirection - distance.normalized;

            if (turnDirection.magnitude > 0.1)
            {
                //boat.transform.eulerAngles = Vector3.RotateTowards(distance, boat.transform.eulerAngles, 2, 0);
                //boat.transform.Rotate(turnDirection);
            }

            float restTravelDistance = boat.GetEstimatedTravelDistanceLeft(extendedSailDirection);

            float extraDistanceFromLowerSailTime = averageSailHeightWhileReducing * boat.weather.windStrength * timeToReduceSailToZero;

            if (restTravelDistance + travelTimeUntilReachingSail + extraDistanceFromLowerSailTime >= distance.magnitude)
            {
                if (boat.SailUpTransition > 0)
                {
                    BringSailsTo(0);
                }
            }
            else if (distance.magnitude - restTravelDistance > 5 && boat.SailUpTransition < 1)
            {
                BringSailsTo(1);
            }

            float speedWithWind = boat.weather.windStrength * boat.sailMaxSize * boat.SailUpTransition;

            if (distance.magnitude < 4f && restTravelDistance < 3.75f && boat.SailUpTransition == 0)
            {
                target = null;
                ReachedDestination();
            }
        }
    }

    protected virtual void ReachedDestination() { }

    private void BringSailsTo(float progress)
    {
        progress = Mathf.Clamp(progress, 0, 1);
        if (changeSailTask != null)
        {
            StopCoroutine(changeSailTask);
        }
        changeSailTask = SailTask(progress);
        StartCoroutine(changeSailTask);
    }

    private float SailsSettingSpeed
    {
        get
        {
            return setSailSpeed / 2;
        }
    }

    private IEnumerator SailTask(float toProgress)
    {
        float neededDiff = toProgress - boat.SailUpTransition;

        ///either 1 or  -1
        int coefficient = (int)(Mathf.Abs(neededDiff) / neededDiff);

        float currentDiff = SailsSettingSpeed * Time.deltaTime * coefficient;

        while (Mathf.Abs(currentDiff) < Mathf.Abs(neededDiff))
        {
            boat.ChangeSailsFor(currentDiff);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        boat.SetSailsTo(toProgress);
        isBusy = false;
    }

    [Save]
    public bool canEnterBoat = true;

    public override void BoatChange(Boat newBoat)
    {
        if (newBoat == null)
        {
            SaveableScene.SetAsRootTransform(transform);
        }
        else
        {
            if (canEnterBoat)
            {
                transform.parent = newBoat.PassengerSeat;
            }
        }
    }

    public override void IsInSettingSailAreaChanged(bool isInside)
    {
        //throw new System.NotImplementedException();
    }

    public override void IsInSteeringAreaChange(bool isInside)
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator changeSailTask;

}
