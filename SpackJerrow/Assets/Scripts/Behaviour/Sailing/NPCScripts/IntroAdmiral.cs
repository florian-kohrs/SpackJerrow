using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAdmiral : NPCNavigateScript
{
    
    public Transform leaveBoatPosition;
    
    protected override void setDataBeforeSaving(PersistentGameDataController.SaveType saveType)
    {
        if (leaveBoatPosition != null)
        {
            leaveBoatPos = new Serializable3DVector(leaveBoatPosition.position);
        }
        base.setDataBeforeSaving(saveType);
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        if (targetPos != null)
        {
            leaveBoatPos = new Serializable3DVector(leaveBoatPosition.position);
        }
        base.OnFirstTimeBehaviourAwakend();
    }

    [Save]
    private Serializable3DVector leaveBoatPos;
    
    public float jumpHeight = 3;

    public AudioSource source;

    public AudioClip talkClip;

    protected override void ReachedDestination()
    {
        canEnterBoat = false;
        StartCoroutine(
            SmoothTransformation<Vector3>.GetStoppable(transform.position, leaveBoatPos.v, 0.8f
            , (v) => transform.position = v, 
            (s, t, p) => 
            {
                Vector3 newPos = s + (t - s) * p;
                newPos.y += Mathf.Sin(p * Mathf.PI) * jumpHeight;
                return newPos;
            }
            , delegate { GetComponent<DialogTrigger>().enabled = true; }));
    }

    public void JumpInWater()
    {
        StartCoroutine(
            SmoothTransformation<Vector3>.GetStoppable(transform.position, target.v, 0.8f
            , (v) => transform.position = v,
            (s, t, p) =>
            {
                Vector3 newPos = s + (t - s) * p;
                newPos.y += Mathf.Sin(p * Mathf.PI) * jumpHeight;
                return newPos;
            }
            , delegate { GetComponent<DialogTrigger>().enabled = true; }));
        Rigidbody r = gameObject.GetOrAddComponent<Rigidbody>();
        r.isKinematic = false;
        r.useGravity = true;
    }

}
