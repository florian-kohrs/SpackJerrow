using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// makes a transform follow another transform
/// this has the advantage from being a child in 
/// scene hirachy that the rotation is ignored
/// </summary>
public class Follow : MonoBehaviour
{

    public Transform target;

    protected Vector3 distance;
    
    private void Start()
    {
        distance = target.transform.position - transform.position;        
    }

    void LateUpdate()
    {
        transform.position = target.transform.position - distance;
    }
}
