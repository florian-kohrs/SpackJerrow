using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipableItem : InventoryItem
{

    public Vector3 equipLocalPosition;
    public Vector3 equipLocalEulerAngle;
    public Vector3 localScale = new Vector3(1,1,1);

}
