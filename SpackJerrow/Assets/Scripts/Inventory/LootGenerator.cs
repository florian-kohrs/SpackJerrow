using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootGenerator
{

    public List<ItemSetPicker> itemSets;
     
    public void AddItemsTo(List<ItemContainer> items)
    {
        foreach(ItemSetPicker picker in itemSets)
        {
            picker.AddItems(items);
        }
    }

    public List<ItemContainer> GetItems()
    {
        List<ItemContainer> items = new List<ItemContainer>();
        AddItemsTo(items);
        return items;
    }

}
