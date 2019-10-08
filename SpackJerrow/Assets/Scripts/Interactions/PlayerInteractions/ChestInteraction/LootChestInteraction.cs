using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class LootChestInteraction : OpenCloseInteraction
{
    
    [Save]
    private List<ItemContainer> items;

    public LootGenerator randomItems;

    public List<ItemContainer> fixedItems;

    protected override void OnFirstTimeBehaviourAwakend()
    {
        items = fixedItems;
        randomItems.AddItemsTo(items);
        base.OnFirstTimeBehaviourAwakend();
    }

    public AudioSource source;

    protected override void OnOpen(GameObject interactor)
    {
        Inventory i = interactor.GetComponent<Inventory>();
        AnimateItem.AnimateItems(items, (int)(transitionSpeed * 0.5f), this);
        i.AddItems(items);
        items.Clear();
    }
    
}
