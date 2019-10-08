using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : WaterRigidBody
{
    
    [HideInInspector]
    public Weather weather;
    
    public Steering rudder;

    public SailTrigger sailTrigger;

    public EnterBoatTrigger passengerTrigger;

    public Vector3 sailPosition;

    public Vector3 sailScale;

    public Transform PassengerSeat => passengerTrigger.transform;

    public float sailMaxSize = 4;

    public float startSailUpTransition;
    
    [Save]
    private float sailUpTransition = 1;

    public Collider sailInteractTrigger;

    private List<Sailor> crew = new List<Sailor>();


    protected new void Start()
    {
        base.Start();
        weather = Environment.GetWeather();
    }

    protected override void BehaviourLoaded()
    {
        sail.transform.localPosition = sailPosition;
        sail.transform.localScale = sailScale;
        SetSailTransform(1,sailUpTransition);
        ///Restore passengers
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        SetSailsTo(startSailUpTransition);
        base.OnFirstTimeBehaviourAwakend();
    }

    #region audio

    [SerializeField]
    private AudioClip sailsDownClip;

    [SerializeField]
    private AudioClip sailsUpClip;

    [SerializeField]
    private AudioClip sailsbackInWindClip;
    
    [SerializeField]
    private AudioSource shipAudioSource;

    [SerializeField]
    private AudioSource shipWaterAudioSource;
    #endregion

    public float SailUpTransition
    {
        get
        {
            return sailUpTransition;
        }
    }
    
    public Transform mast;
    
    public Transform sail;

    public Collider SteerInteractArea
    {
        get
        {
            return rudder.steeringArea.triggerArea;
        }
    }

    public Collider SailInteractArea
    {
        get
        {
            return sailTrigger.triggerArea;
        }
    }

    public Collider PassengerArea
    {
        get
        {
            return passengerTrigger.triggerArea;
        }
    }

    public float maxYScale = 1;

    public override void OnWaterUpdated(Vector3 lastMoveDirection)
    {
        Vector3 rotateDelta = rudder.transform.localEulerAngles;
        if (rotateDelta.y > 180)
        {
            rotateDelta.y -= 360;
        }
        transform.localEulerAngles += rotateDelta * Time.deltaTime * -1;
        
        Velocity.v += new Vector3(0, 0, GetCurrentAcceleration(SailUpTransition));
       
        float waterNoise = Mathf.InverseLerp(2f, 20, Velocity.v.magnitude);
        shipWaterAudioSource.volume = waterNoise;

        foreach(Sailor s in crew)
        {
            Vector3 distance = new Vector3(transform.position.x - s.transform.position.x,0, transform.position.z - s.transform.position.z);
            Vector3 heightDifferenceByDistance = Vector3.Scale(distance, deltaEuler / 15);
            lastMoveDirection.y += -heightDifferenceByDistance.x - heightDifferenceByDistance.z;
            s.ShipMoving(lastMoveDirection);
        }
    }

    public override void StartSinking()
    {
        sunk = true;
        Rigidbody body = GetComponent<Rigidbody>();
        body.isKinematic = false;
        body.velocity = Velocity.v;
        Velocity.v = Vector3.zero;
        body.useGravity = true;
        Destroy(this);
    }

    private bool sunk;

    private float GetCurrentAcceleration(float sailHeight)
    {
        if (!sunk)
        {
            return weather.windStrength * Time.deltaTime * sailHeight * sailMaxSize;
        }
        else
        {
            return 0;
        }
    }

    public float SimulateTravelLength(List<System.Tuple<float, float>> sailStates)
    {
        float result = 0;
        float sailTransition = SailUpTransition;


        return 0;
    }

    public void ChangeSailsFor(float change)
    {
        if (change != 0)
        {
            ///cant higher the sails if they are at 1 already, same for lowering at 0
            if (!((SailUpTransition == 1 && change > 0) || (SailUpTransition == 0 && change < 0)))
            {
                float newValue = Mathf.Clamp01(change + SailUpTransition);

                if (change > 0)
                {
                    PlayAudioIfAvailable(sailsUpClip);
                }
                else
                {
                    PlayAudioIfAvailable(sailsDownClip);
                }
                SetSailsTo(newValue);
            }
        }
    }

    public void SailorBoatStateChanged(Sailor sailor, bool isInBoat)
    {
        if (isInBoat)
        {
            crew.Add(sailor);
            sailor.setNewBoat(this);
        }
        else
        {
            crew.Remove(sailor);
            sailor.setNewBoat(null);
        }
    }

    public void SetSailsTo(float progress)
    {
        SetSailTransform(sailUpTransition, progress);
        //float newDisplayedProgress = Mathf.Max(0.1f, progress);
        //float displayedDifference = newDisplayedProgress - Mathf.Max(0.1f, sailUpTransition);

        //Vector3 newScale = sail.transform.localScale;
        //newScale.y = maxYScale * newDisplayedProgress;
        //sail.transform.localScale = newScale;

        //Vector3 newPosition = sail.transform.localPosition;
        //newPosition.y += maxYScale * displayedDifference / 2;
        //sail.transform.localPosition = newPosition;
        //sailUpTransition = progress;
    }

    protected virtual void SetSailTransform(float fromProgress, float toProgress)
    {
        float newDisplayedProgress = Mathf.Max(0.1f, toProgress);
        float displayedDifference = newDisplayedProgress - Mathf.Max(0.1f, fromProgress);

        Vector3 newScale = sail.transform.localScale;
        newScale.y = maxYScale * newDisplayedProgress;
        sail.transform.localScale = newScale;

        Vector3 newPosition = sail.transform.localPosition;
        newPosition.y += maxYScale * displayedDifference / 2;
        sail.transform.localPosition = newPosition;
        sailUpTransition = toProgress;
    }

    private void PlayAudioIfAvailable(AudioClip clip)
    {
        if (!shipAudioSource.isPlaying)
        {
            shipAudioSource.clip = clip;
            shipAudioSource.Play();
        }
    }

    public void PassengerStateChangedOnSteering(Sailor sailor, bool isInArea)
    {
        sailor.IsInSteeringAreaChange(isInArea);
    }

    public void PassengerStateChangedOnSettingSails(Sailor sailor, bool isInArea)
    {
        sailor.IsInSettingSailAreaChanged(isInArea);
    }
    
}
