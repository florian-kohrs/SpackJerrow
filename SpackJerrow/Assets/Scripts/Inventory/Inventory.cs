using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SaveableMonoBehaviour
{

    /// <summary>
    /// item is key, itemcount is value
    /// </summary>
    [Save]
    public Dictionary<InventoryItemRef, int> items = new Dictionary<InventoryItemRef, int>();

    public List<InventoryItem> startItems;

    [Save]
    private int selectedItemIndex = -1;

    public bool HasItemEquiped
    {
        get
        {
            return equipedItem != null;
        }
    }
    
    private GameObject equipedItem;

    protected override void BehaviourLoaded()
    {
        EquipItem(selectedItemIndex);
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        foreach (InventoryItem i in startItems)
        {
            AddItem(i);
        }
        EquipItem(startItems.Where(item => item is EquipableItem).FirstOrDefault() as EquipableItem);
    }

    public void AddItem(InventoryItem item, int count = 1)
    {
        InventoryItemRef itemRef = new InventoryItemRef();
        itemRef.RuntimeRef = item;
        AddItem(itemRef, count);
    }

    public bool GotItems(IEnumerable<ItemContainer> items)
    {
        bool result = true;
        foreach(ItemContainer i in items)
        {
            result = result && HasItem(i);
        }
        return result;
    }

    public bool HasItem(ItemContainer item)
    {
        return HasItem(item.item,item.itemCount);
    }

    public bool HasItem(InventoryItem i, int count)
    {
        return HasItem(new InventoryItemRef() { RuntimeRef = i }, count);
    }

    public bool HasItem(InventoryItemRef i, int count)
    {
        int itemCount;

        items.TryGetValue(i, out itemCount);

        return itemCount >= count;
    }

    public void AddItems(IEnumerable<ItemContainer> items, int count = 1)
    {
        foreach(ItemContainer i in items)
        {
            AddItem(i);
        }
    }

    public void AddItem(ItemContainer item)
    {
        AddItem(item.item, item.itemCount);
    }

    public void AddItem(InventoryItemRef item, int count = 1)
    {
        if (items.ContainsKey(item))
        {
            items[item] += count;
        }
        else
        {
            items.Add(item,count);
        }
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItems(IEnumerable<ItemContainer> cs)
    {
        foreach(ItemContainer c in cs)
        {
            RemoveItem(c.item, c.itemCount, false);
        }
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveItem(InventoryItem i, int count = 1, bool updateUI = true)
    {
        return RemoveItem(new InventoryItemRef() { RuntimeRef = i }, count, updateUI);
    }

    public bool RemoveItem(InventoryItemRef item, int count = 1, bool callInventoryChanged = true)
    {
        int itemCount;
        bool result = items.TryGetValue(item, out itemCount) && itemCount >= count;
        
        if (result)
        {
            itemCount -= count;
            if(itemCount == 0)
            {
                int itemIndex = 0;
                foreach(InventoryItemRef r in items.Keys)
                {
                    if (r.Equals(item))
                    {
                        break;
                    }
                    itemIndex++;
                }
                if(itemIndex < selectedItemIndex)
                {
                    selectedItemIndex--;
                }
                else if(selectedItemIndex == itemIndex)
                {
                    UnequipCurrentItem();
                }
                //if(item == selectedItem)
                //{
                    
                //}
                items.Remove(item);
            }
            else
            {
                items[item] = itemCount;
            }
            if (callInventoryChanged)
            {
                OnInventoryChanged?.Invoke();
            }
        }

        return result;
    }

    protected System.Action OnInventoryChanged;

    private void UnequipCurrentItem()
    {
        if (equipedItem != null)
        {
            Destroy(equipedItem);
            equipedItem = null;
            selectedItemIndex = -1;
        }
    }
    
    protected void EquipItem(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < items.Keys.Count)
        {
            EquipItem(items.Keys.ToArray()[itemIndex].RuntimeRef as EquipableItem, itemIndex);
        }
    }

    private void EquipItem(EquipableItem e, int itemIndex)
    {
        if (e != null)
        {
            if (e != equipedItem)
            {
                if (HasItemEquiped)
                {
                    Destroy(equipedItem);
                }
                equipedItem = Instantiate(e.gameObject, transform);
                equipedItem.transform.localEulerAngles = e.equipLocalEulerAngle;
                equipedItem.transform.localPosition = e.equipLocalPosition;
                equipedItem.transform.localScale = e.localScale;
                equipedItem.GetComponent<ItemBehaviour>()?.Enable();
            }
            selectedItemIndex = itemIndex;
        }
    }

    protected void EquipItem(EquipableItem e)
    {
        EquipItem(e, items.Keys.Select(r => r.RuntimeRef).ToList().IndexOf(e));
    }
    
}
