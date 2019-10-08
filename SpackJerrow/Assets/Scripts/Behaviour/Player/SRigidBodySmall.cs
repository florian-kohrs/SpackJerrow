using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SRigidBodySmall : SaveableUnityComponent<Rigidbody>
{

    private Serializable3DVector vel;
    private Serializable3DVector angVel;

    protected override void restoreComponent(Rigidbody component)
    {
        component.velocity = vel.v;
        component.angularVelocity = angVel.v;
    }

    protected override void saveComponent(Rigidbody component, PersistentGameDataController.SaveType saveType)
    {
        vel = new Serializable3DVector(component.velocity);
        angVel = new Serializable3DVector(component.angularVelocity);
    }
}
