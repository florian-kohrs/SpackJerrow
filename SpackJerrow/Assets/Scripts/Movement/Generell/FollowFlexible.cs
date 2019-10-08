using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// will have to same effect as setting the object to the targets child in hirachy
/// extepts that the parent rotation will not be applied while this object
/// can still rotate however it wants
/// </summary>
public class FollowFlexible : MonoBehaviour
{

    public Transform target;

    private Vector3 lastTargetsPosition;

    public bool useGlobalPosition;

    private void Start()
    {
        if(target != null)
        {
            StartFollow(target, true);
        }
    }

    public void StartFollow(Transform target, bool globalChange = true)
    {
        enabled = true;
        this.target = target;
        this.useGlobalPosition = globalChange;
        lastTargetsPosition = ReadPosition(target);
        KeepUp();
    }

    public void KeepUp()
    {
        if (target != null)
        {
            Vector3 current = ReadPosition(target);
            Vector3 movementDelta = lastTargetsPosition - current;

            transform.position -= movementDelta;

            lastTargetsPosition = current;
        }
    }

    public void StopFollow()
    {
        target = null;
        enabled = false;
        readPosition = null;
    }

    private System.Func<Transform, Vector3> readPosition;

    private void OnValidate()
    {
        readPosition = null;
    }

    public System.Func<Transform, Vector3> ReadPosition
    {
        get
        {
            if (readPosition == null)
            {
                if (useGlobalPosition)
                {
                    readPosition = (t) => t.position;
                }
                else
                {
                    readPosition = (t) => t.localPosition;
                }
            }
            return readPosition;
        }
    }
    
}
