using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveTowards : SaveableMonoBehaviour
{

    [Save]
    public STransform target;

    public float movePower;

    public Rigidbody r;

    void Update()
    {
        if (target != null)
        {
            r.isKinematic = false;
            Vector3 direction = target.transform.position - transform.position;
            r.AddForce(direction.normalized * Time.deltaTime * movePower);
        }
    }
}
