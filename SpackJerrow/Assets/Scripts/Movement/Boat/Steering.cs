using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class Steering : MonoBehaviour
{

    [SerializeField]
    private Vector3 defaultEuler = Vector3.zero;

    public Transform rudderRelationPoint;

    public SteeringTrigger steeringArea;

    public float LocalYEuler
    {
        get
        {
            return transform.localEulerAngles.y;
        }
        set
        {
            frozen = true;
            if(currentUnfreezer != null)
            {
                StopCoroutine(currentUnfreezer);
            }
            currentUnfreezer = Unfreeze();
            StartCoroutine(currentUnfreezer);
            float newValue = value;
            if(newValue < 0)
            {
                newValue += 360;
            }else if(newValue > 50 && newValue < 180)
            {
                newValue = 50;
            }else  if(newValue > 180 && newValue < 310)
            {
                newValue = 310;
            }
            Vector3 newLocalEuler = transform.localEulerAngles;
            newLocalEuler.y = newValue;
            transform.localEulerAngles = newLocalEuler;
        }
    }

    public void ChangeRudderRotation(float changeValue)
    {
        LocalYEuler = LocalYEuler + changeValue;
    }

    private IEnumerator currentUnfreezer;

    private IEnumerator Unfreeze()
    {
        yield return new WaitForSeconds(freezeTimeAfterUsing);
        frozen = false;
    }

    public float dragToDefault = 1;

    public float freezeTimeAfterUsing = 0.5f;

    private bool frozen = false;

    /// <summary>
    /// 1 or -1 depending on which side of the rudder the player stands
    /// </summary>
    /// <param name="sailorPos"></param>
    /// <returns></returns>
    public int AngleSignOfTheSailorRelativeToTheRudder(Vector3 sailorPos)
    {
        float signedAngle = Vector3.SignedAngle(transform.position - rudderRelationPoint.position, sailorPos - rudderRelationPoint.position, transform.parent.forward);
        return (int)Mathf.Sign(signedAngle);
    }
    
    private void FixedUpdate()
    {
        if (!frozen)
        {
            Vector3 localEuler = transform.localEulerAngles;
            if (localEuler.y > 180)
            {
                localEuler.y -= 360;
            }

            Vector3 rotateDelta = defaultEuler - localEuler;

            rotateDelta *= 1 - dragToDefault;
            transform.localEulerAngles += rotateDelta * Time.deltaTime;
        }
    }


}
