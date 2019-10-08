using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{

    public Vector3 shotPosition;

    public float cannonBallForce;

    private void OnCollisionEnter(Collision collision)
    {
        Boat waterObject = collision.gameObject.GetComponentInParent<Boat>();
        if (waterObject != null/* && firedBy != waterobject*/)
        {
            waterObject.Velocity.v += (shotPosition - collision.transform.position).normalized * cannonBallForce / 10;
            waterObject.Mass += 100;
            Destroy(this);
        }
        else
        {
            IHealthController h = collision.gameObject.GetInterface<IHealthController>();
            if (h != null)
            {
                h.Damage(15);
                Destroy(this);
            }
        }
        
    }

}
