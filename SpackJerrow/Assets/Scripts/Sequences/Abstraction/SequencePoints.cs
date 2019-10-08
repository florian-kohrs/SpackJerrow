using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SequencePoints
{

    public Transform endHere;

    public float moveToInTime;

    public Vector3 objectRotation;

    public float rotateInTime;

    IEnumerator e;

    IEnumerator t;

    public void Abort()
    {
        if (e != null)
        {
            b.StopCoroutine(e);
        }
        if (t != null)
        {
            b.StopCoroutine(t);
        }
    }

    private MonoBehaviour b;

    public void StartSequencePart(MonoBehaviour behaviour, Transform target, Action onFinish)
    {
        b = behaviour;
        Action callBack = null;

        if (moveToInTime > rotateInTime)
        {
            callBack = onFinish;
        }

        e = SmoothVectorTransformation.GetStoppable
            (
            () => new Vector3[] { target.position },
            (v) => target.position = v,
            endHere.position,
            moveToInTime,
            callBack
            );
        behaviour.StartCoroutine(e);


        if (rotateInTime != 0)
        {

            if (moveToInTime <= rotateInTime)
            {
                callBack = onFinish;
            }
            else
            {
                callBack = null;
            }

            t =
               SmoothTransformation<Vector3>.SmoothRotateAngleEuler(
                   target.localEulerAngles,
                   objectRotation,
                   rotateInTime,
                   v => target.localEulerAngles = v,
                   callBack);


            behaviour.StartCoroutine(t);

        }

    }

}
