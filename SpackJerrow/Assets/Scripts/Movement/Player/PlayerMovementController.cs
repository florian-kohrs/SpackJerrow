using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMovementController : MonoBehaviour
{

    public Rigidbody body;

    public float power = 400;

    public virtual Transform DirectionTransform()
    {
        return transform;
    }

    public abstract Vector3 InputForce { get; }

    public abstract Vector3 LookDiretionInput { get; }

    public Vector3 CurrentInputDirection
    {
        get
        {
            float xForce = Input.GetAxis("Horizontal");
            float yForce = Input.GetAxis("Vertical");
            return new Vector3(xForce, 0, yForce).normalized;
        }
    }

}
