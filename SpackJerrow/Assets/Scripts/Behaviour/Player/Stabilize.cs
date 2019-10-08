using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilize : MonoBehaviour
{

    public Rigidbody rigidBody;

    public float angularBreakSpeed;
    public float breakSpeed;
    public float stabilizeSpeed;

    private void Start()
    {
        if(rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (GameManager.AllowPlayerMovement && Input.GetButton("Stabilize"))
        {
            if(rigidBody != null)
            {
                Vector3 newVelocity = rigidBody.velocity - rigidBody.velocity * (breakSpeed * Time.deltaTime);
                newVelocity.y = rigidBody.velocity.y;
                
                rigidBody.velocity = newVelocity;
                rigidBody.angularVelocity -= rigidBody.angularVelocity * (angularBreakSpeed * Time.deltaTime);
            }
            //float newXEuler = transform.localEulerAngles.x;
            //if (newXEuler > 180)
            //{
            //    newXEuler -= 360;
            //}
            //newXEuler = newXEuler * (1 - (stabilizeSpeed * Time.deltaTime));

            //float newZEuler = transform.localEulerAngles.z;
            //if (newZEuler > 180)
            //{
            //    newZEuler -= 360;
            //}
            //newZEuler = newZEuler * (1 - (stabilizeSpeed * Time.deltaTime));
            //transform.localEulerAngles = new Vector3(newXEuler, transform.localEulerAngles.y, newZEuler);
            Vector3 newEulerAngle = transform.eulerAngles;
            newEulerAngle.x = Mathf.LerpAngle(newEulerAngle.x, 0, (stabilizeSpeed * Time.deltaTime) / 5);
            newEulerAngle.z = Mathf.LerpAngle(newEulerAngle.z, 0, (stabilizeSpeed * Time.deltaTime) / 5);
            newEulerAngle.y = Mathf.LerpAngle(newEulerAngle.y, Camera.main.transform.eulerAngles.y, (stabilizeSpeed * Time.deltaTime) / 5);
            transform.eulerAngles = newEulerAngle;
        }
    }

}
