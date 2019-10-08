using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{

    private bool wasEquiped;

    protected void Start()
    {
        if (!wasEquiped)
        {
            enabled = false;
        }
    }

    public void Enable()
    {
        wasEquiped = true;
        enabled = true;
    }

}
