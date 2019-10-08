using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SetController<T> : MonoBehaviour
{
    
    public abstract void AddElement(T element);

    public abstract void RemoveElement(T element);

}
