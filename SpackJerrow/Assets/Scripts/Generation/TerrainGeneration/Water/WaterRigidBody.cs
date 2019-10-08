using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRigidBody : SaveableMonoBehaviour, IMovementPredicter
{

    public enum FloatBehaviour { Realistic, OnPoint}

    public enum FloatHeightPrecision { Approximately, Exact}

    public FloatHeightPrecision floatPrecision = FloatHeightPrecision.Approximately;

    public FloatBehaviour floatBehaviour;

    public bool canFillWithWater;
    [Tooltip("Increasing the precision of integration increases the performance")]
    [Range(5,1000)]
    public int integrationPrecision = 10;

    [Tooltip("Offset the anchor of the object so that it is centered")]
    public Vector3 offset;

    [Range(0.1f,100f)]
    public float volume = 1;
    
    [Tooltip("The distribution of the volume from y = 0 to y = 1")]
    public AnimationCurve volumeDistribution;

    [Range(0.1f,50000)]
    
    [SerializeField]
    [Save]
    protected float mass = 1;

    [HideInInspector]
    [Save]
    public float startMass;

    protected override void OnFirstTimeBehaviourAwakend()
    {
        startMass = mass;
    }

    public float drag;

    protected void Start()
    {
        Environment.GetSea().AddWaterObject(this);
    }

    protected override void onDestroy()
    {
        if (Environment.HasSea())
        {
            Environment.GetSea().RemoveWaterObject(this);
        }
    }


    public float Density
    {
        get
        {
            return mass / 1000 / volume;
        }
    }
    
    public virtual float Mass
    {
        get
        {
            return mass;
        }
        set
        {
            mass = value;
            if (Density > 1)
            {
                StartSinking();
            }
        }
    }
    
    public float maxWidth;

    public float maxHeight;

    public float maxLength;

    [Range(0.05f, 1)]
    public float floatAccuracy = 1;

    public float GetEstimatedTravelDistanceLeft(float movementLeftTreshhold = 0.1f)
    {
        return GetEstimatedTravelDistanceLeft(Velocity.v,movementLeftTreshhold);
    }

    public float GetEstimatedTravelDistanceLeft(Vector3 direction, float movementLeftTreshhold = 0.1f)
    {
        float result = 0;

        if (drag != 0)
        {

            Vector2 travelDirection = new Vector2(direction.x, direction.z);
            float travelLength = travelDirection.magnitude;

            while (travelLength > movementLeftTreshhold)
            {
                result += travelLength;
                travelLength *= 1 - drag;
            }
        }

        return result;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "CannonBall")
        {
            //Debug.Log(collision.relativeVelocity);
        }
    }

    [Tooltip("Will rotate the object on the current wave slope")]
    public bool rotateOnWave = true;

    [Save]
    [SerializeField]
    private Serializable3DVector velocity;
    
    public virtual Serializable3DVector Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }

    private void OnValidate()
    {
        DensityIntegrationFaktor = volume / volumeDistribution.Integrate(0, 1, integrationPrecision);
    }

    public Vector3 CurrentPosition
    {
        get
        {
            return transform.position + offset;
        }
    }

    public float DensityIntegrationFaktor { get; private set; }
    
    public SeaSimulator FloatController { get; set; }

    public float ActualDragByWater
    {
        get
        {
            if(lastInfo != null)
            {
                return drag * lastInfo.sizePercentageInWater;
            }
            else
            {
                return drag;
            }
        }
    }

    public virtual void StartSinking()
    {
        drag *= 2;
    }

    /// <summary>
    /// this is called every time after the water was updated
    /// </summary>
    public virtual void OnWaterUpdated(Vector3 lastMoveDirection) { }
    
    public virtual void OnWaterUpdated()
    {
        Vector3 lastPosition = transform.position;
        Vector3 moveVector = Velocity.v * Time.deltaTime;
        ApplyVelocity(moveVector);
        Vector3 deltaPosition = transform.position - lastPosition;

        if (lastInfo != null)
        {
            Velocity.v *= 1 - (drag * lastInfo.sizePercentageInWater * Time.deltaTime);
        }
        if (Velocity.v.magnitude < 0.0002f)
        {
            Velocity.v = Vector3.zero;
        }
        OnWaterUpdated(deltaPosition);
    }

    protected virtual void ApplyVelocity(Vector3 vel)
    {
        transform.Translate(vel);
    }
    
    private WaterFloatInfo lastInfo;

    private float airTimer = 0;

    protected Vector3 deltaEuler;

    

    public void ApplyFloatInfo(WaterFloatInfo info)
    {
        ApplyFloatData(info);
        lastInfo = info;
    }

    protected virtual void ApplyFloatData(WaterFloatInfo info)
    {
        if (floatBehaviour == FloatBehaviour.Realistic)
        {
            if (info.sizePercentageInWater < 0.15f)
            {
                Velocity.v += new Vector3(0, -9.81f * airTimer * airTimer * Time.deltaTime, 0);
                airTimer += Time.deltaTime;
            }
            else
            {

                float heightDistance = info.height - transform.position.y;
                if (lastInfo != null && (lastInfo.sizePercentageInWater < 0.15f))
                {
                    if (Velocity.v.magnitude > 1)
                    {
                        float splashScale = 1;
                        float splashFaktor = splashScale / Mathf.Max(splashScale, (maxLength * maxWidth));
                        Vector3 splashSize = Velocity.v * mass * (1 - splashFaktor);
                        Velocity.v *= splashFaktor;
                        Debug.Log("Splash: " + splashSize.magnitude);
                    }
                }
                float yForce;

                yForce = (-9.81f + (9.81f * Mathf.Pow(info.densityDifferenceFactor, -1))) * Time.deltaTime * (heightDistance / 2);

                Velocity.v += new Vector3(0, yForce, 0);

                airTimer = 0;
            }
        }
        else if (floatBehaviour == FloatBehaviour.OnPoint)
        {
            transform.position = new Vector3(transform.position.x, info.height, transform.position.z);
        }

        if (rotateOnWave)
        {
            deltaEuler = new Vector3(AngleDistance(info.newEulerAngle.x, transform.eulerAngles.x), 0, AngleDistance(info.newEulerAngle.x, transform.eulerAngles.x));
            transform.eulerAngles = info.newEulerAngle;
        }

    }

    private float AngleDistance(float x, float y)
    {
        while(x < 0)
        {
            x += 360;
        }
        while (y < 0)
        {
            y += 360;
        }
        float result = x - y;
        while (result < 0)
        {
            result += 360;
        }
        if (result > 360)
        {
            result = result % 360;
        }
        if(result > 180)
        {
            result -= 360;
        }
        return result;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetDirection()
    {
        return transform.TransformDirection(Velocity);
       // return Velocity.v;
    }
}
