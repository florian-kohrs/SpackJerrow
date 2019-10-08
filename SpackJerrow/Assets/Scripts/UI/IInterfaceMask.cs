using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterfaceMask
{

    void Open();

    void Close();
    
    CursorLockMode CursorMode { get; }
    
}
