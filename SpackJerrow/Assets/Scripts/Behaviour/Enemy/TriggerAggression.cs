using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerAggression<T> : SaveableMonoBehaviour where T : Component
{

    protected T target;

    protected virtual void StartFocus(T target) { }

    protected virtual void LooseFocus() { }

    protected abstract bool TriggerEvaluator(GameObject o, T c);

    protected Vector3 DirectionToTarget
    {
        get
        {
            if (target == null)
            {
                return Vector3.zero;
            }
            else
            {
                return target.transform.position - transform.position;
            }
        }
    }

    protected float DistanceToTarget
    {
        get
        {
            return DirectionToTarget.magnitude;
        }
    }

    protected float SqrDistanceToTarget
    {
        get
        {
            return DirectionToTarget.sqrMagnitude;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        T t = other.GetComponent<T>();
        if (t != null && 
            TriggerEvaluator(other.gameObject,t))
        {
            Focus(t);
        }
    }

    protected void Focus(T target)
    {
        this.target = target;
        StartFocus(target);
    }

    protected virtual bool EvaluateAggressionLoss()
    {
        return true;
    }

    protected void OnTriggerExit(Collider other)
    {
        if (target != null && other.gameObject == target.gameObject && EvaluateAggressionLoss())
        {
            AbortAggression();
        }
    }

    public void AbortAggression()
    {
        target = null;
        LooseFocus();
    }

}
