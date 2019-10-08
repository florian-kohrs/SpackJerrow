using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


[System.Serializable]
public class InventoryItemRef : AssetPolyRef<InventoryItem>
{
    
    public InventoryItemRef() : base() { }

    protected InventoryItemRef(SerializationInfo info, StreamingContext context) : base(info, context) { }

}
