using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollow : MonoBehaviour
{

    public Transform target;

    public Vector3 distance;

    public bool useSetDistance;
    
    private void Start()
    {
        if (!useSetDistance)
        {
            distance = target.transform.position - transform.position;
        }
    }

    void FixedUpdate()
    {
        transform.position = target.transform.position - distance;
    }

}
