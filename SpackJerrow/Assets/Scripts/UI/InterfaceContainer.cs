using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InterfaceContainer : IInterfaceMask
{
    
    public InterfaceContainer(CursorLockMode mode)
    {
        this.mode = mode;
    }

    public void Open()
    {
        uiMask.SetActive(true);
    }

    public void Close()
    {
        uiMask.SetActive(false);
    }

    public GameObject uiMask;

    private CursorLockMode mode;

    public CursorLockMode CursorMode => mode;

}
