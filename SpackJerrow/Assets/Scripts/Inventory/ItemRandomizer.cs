using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRandomizer
{

    public ItemRandomizer()
    {
        itemCountDistribution = new AnimationCurve();
        itemCountDistribution.AddKey(new Keyframe(0,1));
        itemCountDistribution.AddKey(new Keyframe(1,1));
    }

    public InventoryItem item;

    public int minItemCount = 0;

    public int maxItemCount = 1;

    public AnimationCurve itemCountDistribution;
    
    public void AddItems(List<ItemContainer> allItems)
    {
        float maxPercentage = 
            itemCountDistribution.Integrate(0, 1, itemCountDistribution.keys.Length * 5);
       
        float itemCountP = itemCountDistribution.IntegrateUntil(0, Random.Range(0,maxPercentage), itemCountDistribution.keys.Length * 5);
        int itemCount = Mathf.RoundToInt(minItemCount + ((maxItemCount - minItemCount) * itemCountP));
        Debug.Log("Added Item count: " + itemCount + ". Min was " + minItemCount + ", max was:" + maxItemCount);
        if (itemCount > 0)
        {
            allItems.Add(new ItemContainer(item, itemCount));
        }
    }

}
