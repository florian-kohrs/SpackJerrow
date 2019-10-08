using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer
{

    public ItemContainer() { }

    public ItemContainer(InventoryItem item, int itemCount)
    {
        this.item = new InventoryItemRef() { RuntimeRef = item };
        this.itemCount = itemCount;
    }

    public InventoryItemRef item;

    public int itemCount;

}
