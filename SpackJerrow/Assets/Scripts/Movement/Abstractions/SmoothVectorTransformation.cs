using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// starts a coroutine for a number of objects transforming a vector over time
/// </summary>
public class SmoothVectorTransformation
{

    #region fixed type
    private SmoothVectorTransformation() { }

    public static void StartUnstoppable(MonoBehaviour b, Func<Vector3[]> getVectors,
        Action<Vector3> setVector, Vector3 endHere, float finishInTime, Action onFinish = null)
    {
        b.StartCoroutine(new SmoothVectorTransformation().startUnstoppable(getVectors, setVector, endHere, finishInTime, onFinish));
    }

    public static IEnumerator GetStoppable(Func<Vector3[]> getVectors,
    Action<Vector3> setVector, Vector3 endHere, float finishInTime, Action onFinish = null)
    {
        return new SmoothVectorTransformation().startUnstoppable(getVectors, setVector, endHere, finishInTime, onFinish);
    }

    private float timeLeft;
    private Vector3 endHere;
    private List<Vector3> speeds = new List<Vector3>();
    private Func<Vector3[]> getVectors;
    private Action<Vector3> setVector;

    public Action onFinish;

    public IEnumerator startUnstoppable(Func<Vector3[]> getVectors, 
        Action<Vector3> setVector, Vector3 endHere, float finishInTime, Action onFinish)
    {
        timeLeft = finishInTime;
        this.endHere = endHere;
        this.getVectors = getVectors;
        this.setVector = setVector;
        this.onFinish = onFinish;
        foreach (Vector3 v in getVectors())
        {
            Vector3 dist = endHere - v;
            speeds.Add(dist.normalized * (dist.magnitude / finishInTime));
        }
        return smooth();
    }

    private IEnumerator smooth()
    {
        while(timeLeft > 0)
        {
            if (GameManager.AllowMovement)
            {
                int count = 0;
                foreach (Vector3 current in getVectors())
                {
                    Vector3 delta = speeds[count] * Time.deltaTime;
                    Vector3 newVector = current + delta;
                    setVector(newVector);
                    timeLeft -= Time.deltaTime;
                    count++;
                }
            }
            yield return null;
        }
        setVector(endHere);
        if(onFinish != null)
        {
            onFinish();
        }
    }
#endregion


}
