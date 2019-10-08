using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterfaceMask : MonoBehaviour, IInterfaceMask
{

    public GameObject maskRoot;

    protected abstract void Open_();

    public void Open()
    {
        maskRoot.SetActive(true);
        Open_();
    }

    protected virtual void Start()
    {
        maskRoot.SetActive(false);
    }

    public virtual CursorLockMode CursorMode => CursorLockMode.Locked;
    
    public int layer;

    public virtual bool BlockOtherInterfaces => false;
    
    public virtual bool DominantMask => false;

    protected abstract void Close_();

    public void Close()
    {
        maskRoot.SetActive(false);
        Close_();
    }

}
