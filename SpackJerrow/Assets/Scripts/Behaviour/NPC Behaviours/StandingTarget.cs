using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingTarget : MonoBehaviour, IMovementPredicter
{
    public Vector3 GetDirection()
    {
        return Vector3.zero;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
