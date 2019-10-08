using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChestInfo
{

    [System.NonSerialized]
    public BurriedChestInteraction chest;

    public Serializable2DVector locationProgress;

    public Serializable2DVector localPosition;
    
    public bool markChest;

    [System.NonSerialized]
    public bool moveChest;
    
}
