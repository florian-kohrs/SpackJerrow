using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class ControllableCoroutine<T> : IControllableCoroutine
{

    public ControllableCoroutine(T startValue, T endValue, float timeTillFinish, Action<T> setter, Func<T,T,float,T> valueBuilder, Action onFinish = null)
    {
        this.setter = setter;
        this.valueBuilder = valueBuilder;
        this.onFinishCallback = onFinish;
        this.endValue = endValue;
        this.startValue = startValue;
        timeLeft = timeTillFinish;
        maxTime = timeTillFinish;
    }

    protected T currentValue;

    protected T startValue;

    protected T endValue;

    protected float timeLeft;

    [NonSerialized]
    protected Action<T> setter;

    [NonSerialized]
    protected Func<T, T, float, T> valueBuilder;

    protected bool isPlaying;

    protected IEnumerator GetUpdater()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                timeLeft = 0;
            }
            currentValue = valueBuilder(startValue, endValue, 1 - (timeLeft / maxTime));
            setter(currentValue);
            yield return null;
        }
        OnFinish();
    }

    protected Action onFinishCallback;

    protected float maxTime;

    protected IEnumerator animator;

    protected MonoBehaviour source;

    public bool IsPlaying => isPlaying;

    public void Stop()
    {
        isPlaying = false;
        if (animator != null)
        {
            source.StopCoroutine(animator);
        }
    }

    public void Start(MonoBehaviour source)
    {
        isPlaying = true;
        if(animator != null)
        {
            this.source.StopCoroutine(animator);
        }
        animator = GetUpdater();
        this.source = source;
        source.StartCoroutine(animator);
    }

    protected void OnFinish()
    {
        setter(endValue);
        onFinishCallback?.Invoke();
    }

    public void Finish()
    {
        Stop();
        OnFinish();
    }
}
