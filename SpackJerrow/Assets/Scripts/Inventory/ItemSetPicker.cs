using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSetPicker
{

    public int minItemSet = 0;

    public int maxItemSet = 1;

    public List<ItemRandomizer> itemSets;

    public void AddItems(List<ItemContainer> allItems)
    {
        foreach(ItemRandomizer r in Rand.GetRandomUniqueSet(itemSets, 
            Random.Range(minItemSet, Mathf.Min(maxItemSet, itemSets.Count))))
        {
            r.AddItems(allItems);
        }
    }

}
