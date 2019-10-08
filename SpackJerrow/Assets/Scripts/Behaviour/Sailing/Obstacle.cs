using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is the needed script when an water object shall collide with this object
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{

    [Tooltip("Sharpness is simular to damage")]
    public float sharpness = 0.05f;
    
    [Tooltip("The ship will bounce back by its current velocity times this value")]
    [Range(0,10)]
    public float bouncePower = 1;

    private float minKnockbackPower = 2.25f;

    private List<GameObject> collided = new List<GameObject>(); 
    
    private void OnCollisionExit(Collision collision)
    {
        collided.Remove(collision.gameObject);
    }
    
    protected virtual Vector3 NewVelocity(WaterRigidBody w)
    {
        //Vector3 reflectVel = (m.GetPosition() - transform.position).normalized * m.GetDirection().magnitude / 2;
        //float bounce = -bouncePower / 2;
        //Vector3 bounceVel = m.GetDirection() * bounce;
        //return reflectVel + bounceVel;

        Vector3 crashDir = transform.position - w.transform.position;
        Vector3 moveDir = w.transform.TransformDirection(w.Velocity.v);

        Vector3 result = w.Velocity.v;
        float angle = Vector3.Angle(crashDir, moveDir);
        Debug.Log(angle);
        if (angle < 105)
        {
            result = w.Velocity.v * -1 * bouncePower;
            if (result.magnitude < minKnockbackPower)
            {
                result = result.normalized * minKnockbackPower;
            }
        }
        return result;
    }

    protected virtual float CrashImpact(WaterRigidBody w)
    {
        return w.Velocity.v.magnitude;
    }

    private void OnCollisionEnter(Collision collision)
    {
        WaterRigidBody waterBody = collision.gameObject.GetComponent<WaterRigidBody>();
        if (waterBody != null)
        {
            if (waterBody.Density <= 1 && !collided.Contains(collision.gameObject))
            {

                Vector3 firstContactPoint = collision.GetContact(0).point;
                Vector3 contactDirection = firstContactPoint - waterBody.transform.position;
                firstContactPoint.y = 0;

                Vector3 travelDirection = waterBody.transform.TransformDirection(waterBody.Velocity.v);
                travelDirection.y = 0;

                float collisionAngle = Vector3.Angle(contactDirection, travelDirection);

                waterBody.Velocity = NewVelocity(waterBody);
                float damage = CrashImpact(waterBody) * sharpness * Mathf.Cos(collisionAngle / 360 * Mathf.PI) * waterBody.Mass;
                waterBody.Mass += damage;
                Debug.Log("Angle: " + collisionAngle);
                Debug.Log("Damage: " + damage);
                collided.Add(collision.gameObject);
            }
        }
    }

}
