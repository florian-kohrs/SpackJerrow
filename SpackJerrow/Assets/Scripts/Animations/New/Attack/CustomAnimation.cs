using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomAnimation<T,J> : ScriptableObject, IAnimation<T, J> where T : BaseAnimation
{
    
    public List<T> animParts;

    [System.NonSerialized]
    private int currentIndex;

    [System.NonSerialized]
    private float currentAnimRestTime;

    [System.NonSerialized]
    private float timeSinceStart;
    
    public bool IsAnimationFinish => currentIndex >= animParts.Count;
    
    public T CurrentAnimPart => animParts[currentIndex];

    protected void SetAnimValuesAtTime(float time)
    {
        int result = -1;
        float restTime = time;
        do
        {
            result++;
            restTime -= animParts[result].neededTime;
        } while (restTime >= 0);
        
        currentIndex = result;
        timeSinceStart = time;
        currentAnimRestTime = restTime * -1;
    }

    protected void IncreaseAnimStepIfNeeded()
    {
        while (currentAnimRestTime <= 0)
        {
            currentIndex++;
            if (!IsAnimationFinish)
            {
                currentAnimRestTime += CurrentAnimPart.neededTime;
            }
        }
    }
    
    public bool UpdateAnim(float deltaTime)
    {
        currentAnimRestTime -= deltaTime;
        timeSinceStart += deltaTime;
        IncreaseAnimStepIfNeeded();
        UpdateAnimatedValue();
        return IsAnimationFinish;
    }

    public void StartAnimationAt(float time)
    {
        SetAnimValuesAtTime(time);
    }

    protected abstract void UpdateAnimatedValue();

    public abstract void SetValue(T view, J source);

}
