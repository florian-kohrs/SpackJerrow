using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransformation<V>
{

    private SmoothTransformation() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="setter"></param>
    /// <param name="transformStep">first arg is startValue, second is targetValue, last is finishProgress, needs currentValue as return</param>
    /// <param name="endValue"></param>
    /// <param name="finishInTime"></param>
    /// <returns></returns>
    public static IEnumerator GetStoppable(V startValue, V endValue, float finishInTime, Action<V> setter, Func<V, V, float, V> transformStep, Action onFinish = null)
    {
        return new SmoothTransformation<V>().GetEnumerator(setter, transformStep, startValue, endValue, finishInTime, onFinish);
    }

    private float timeLeft;
    private V targetView;
    private V currentView;
    private V startView;
    private Action<V> setNewValue;
    private Func<V, V, float, V> transformToNext;
    private Action onFinish;
    private float maxTime;

    public IEnumerator GetEnumerator(Action<V> setter, Func<V, V, float, V> transformToNext, V startValue, V goTo, float finishInTime, Action onFinish)
    {
        timeLeft = finishInTime;
        startView = startValue;
        currentView = startValue;
        this.maxTime = finishInTime;
        this.targetView = goTo;
        this.setNewValue = setter;
        this.transformToNext = transformToNext;
        this.onFinish = onFinish;
        return smooth();
    }

    private IEnumerator smooth()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
            }
            currentView = transformToNext(startView, targetView, 1 - (timeLeft / maxTime));
            setNewValue(currentView);
            yield return null;
        }
        setNewValue(targetView);
        if (onFinish != null)
        {
            onFinish();
        }
    }

    public static IEnumerator SmoothRotateAngleEuler(Vector3 start, Vector3 toRotation, float finishedInTime, Action<Vector3> set, Action onFinish = null)
    {
        return SmoothTransformation<Vector3>.GetStoppable(start, toRotation, finishedInTime, set, (s, t, p) => new Vector3(Mathf.LerpAngle(s.x, t.x, p), Mathf.LerpAngle(s.y, t.y, p), Mathf.LerpAngle(s.z, t.z, p)), onFinish);
    }

    public static IEnumerator SmoothRotateEuler(Vector3 start, Vector3 toRotation, float finishedInTime, Action<Vector3> set, Action onFinish = null)
    {
        return SmoothTransformation<Vector3>.GetStoppable(start, toRotation, finishedInTime, set, (s, t, p) => new Vector3(Mathf.Lerp(s.x, t.x, p), Mathf.Lerp(s.y, t.y, p), Mathf.Lerp(s.z, t.z, p)), onFinish);
    }

}
