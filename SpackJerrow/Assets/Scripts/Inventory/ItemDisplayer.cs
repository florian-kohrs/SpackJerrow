using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ItemDisplayer
{

    protected List<InventorySlot> items;

    private static InventorySlot GetSlot(int index, List<InventorySlot> slots, GameObject slotPrefab, Transform slotParent)
    {

        InventorySlot slot;
        if (index >= slots.Count)
        {
            slot = GameObject.Instantiate(slotPrefab).GetComponent<InventorySlot>();
            slots.Add(slot);
            slot.transform.SetParent(slotParent, false);
        }
        else
        {
            slot = slots[index];
            slot.gameObject.SetActive(true);
        }
        return slot;
    }

    public static void Fill<T>(Rect rect, GameObject slotPrefab,
        Transform slotParent, Dictionary<T, int> items,
        List<InventorySlot> slots, int inventorySlotSize, int inventorySlotSpace,
        System.Action<T> clickEvent, System.Action<T> hoverEvent,
        System.Action<T> hoverExitEvent, System.Func<T, bool> filter = null)
        where T : InventoryItemRef
    {
        int counter = 0;
        InventorySlot s;
        Vector2 uiSize = rect.max - rect.min;
        int slotDistance = inventorySlotSize + inventorySlotSpace;
        int startX = inventorySlotSpace + inventorySlotSize / 2;
        int currentX = startX;
        int currentY = currentX;
        int uiXSize = (int)uiSize.x + inventorySlotSize / 2;
        foreach (KeyValuePair<T, int> p in items)
        {
            if (filter == null || filter(p.Key))
            {
                s = GetSlot(counter, slots, slotPrefab, slotParent);
                s.gameObject.SetActive(true);
                s.itemCount.text = p.Value.ToString();
                s.itemSprite.sprite = p.Key.RuntimeRef.icon;
                s.btn = s.GetOrAddComponent<Button>();

                ///remove previous click events & add new one
                s.btn.onClick.RemoveAllListeners();
                s.btn.onClick.AddListener(delegate { clickEvent?.Invoke(p.Key); });

                RectTransform t = s.GetComponent<RectTransform>();
                t.anchorMax = new Vector2(0, 1);
                t.anchorMin = new Vector2(0, 1);
                t.anchoredPosition = new Vector2(currentX, -currentY);
                currentX += slotDistance;
                if (currentX + inventorySlotSize > uiXSize)
                {
                    currentX = startX;
                    currentY += slotDistance;
                }

                t.GetOrAddComponent<BoxCollider2D>();

                ///add hover event
                if (hoverEvent != null)
                {
                    EventTrigger trigger = s.btn.GetOrAddComponent<EventTrigger>();
                    trigger.triggers.Clear();
                    Entry entry = new Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback.AddListener(eventData => { hoverEvent(p.Key); });
                    trigger.triggers.Add(entry);

                    entry = new Entry();
                    entry.eventID = EventTriggerType.PointerExit;
                    entry.callback.AddListener(e => hoverExitEvent(p.Key));
                    trigger.triggers.Add(entry);
                }
                
                counter++;

            }
        }

        ///disable all unused slots
        while (counter < slots.Count)
        {
            slots[counter].gameObject.SetActive(false);
            counter++;
        }
    }

        public static void Fill<T>(Rect rect, GameObject slotPrefab, 
        Transform slotParent, Dictionary<T, int> items, 
        List<InventorySlot> slots, int inventorySlotSize, int inventorySlotSpace, 
        System.Action<T> clickEvent = null, System.Func<T, bool> filter = null) 
        where T: InventoryItemRef
    {
        Fill(rect, slotPrefab, slotParent, items, slots, inventorySlotSize, 
            inventorySlotSpace, clickEvent, null, null, filter);
    }

}
