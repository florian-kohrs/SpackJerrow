using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MarksmanAgression<T, I> : TriggerAggression<T> where T : Component, I where I : class, IMovementPredicter
{

    protected I targetMovement;

    protected sealed override void StartFocus(T target)
    {
        targetMovement = target as I;
        SwitchedTargetTo(target);
    }

    protected virtual void SwitchedTargetTo(T target) { }

}
