using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : Inventory, IInterfaceMask
{
    
    public GameObject slotPrefab;

    private List<InventorySlot> slots = new List<InventorySlot>();
    
    public List<Image> inventorySprites;
    
    private List<EquipableItem> currentDisplayedItems = new List<EquipableItem>();
    
    public const int INVENTORY_SLOT_SIZE = 100;

    public const int INVENTORY_SLOT_SPACE = 5;

    private int SlotDistance => INVENTORY_SLOT_SIZE + INVENTORY_SLOT_SPACE;

    public InterfaceController interfaceController;

    private Rect r;

    private Rect R
    {
        get
        {
            if(r == default)
            {
                r = uiMask.GetComponent<RectTransform>().rect;
            }
            return r;
        }
    }

    private Vector2 InventorySize
    {
        get
        {
            return R.max - R.min; 
        }
    }

    protected override void BehaviourLoaded()
    {
        UpdateCurrentDisplayedItems();
        base.BehaviourLoaded();
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        base.OnFirstTimeBehaviourAwakend();
    }

    protected void InventoryChanged()
    {
        UpdateCurrentDisplayedItems();
    }

    public override void OnAwake()
    {
        OnInventoryChanged = InventoryChanged;
        base.OnAwake();
    }

    private void Start()
    {
        uiMask.SetActive(false);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && GameManager.AllowPlayerActions)
        {
            if (IsOpen)
            {
                interfaceController.RemoveMask_(this);
            }
            else
            {
                interfaceController.AddMask_(this);
            }
        }
        ClickEvent();
    }

    protected void ClickEvent()
    {
        if (GameManager.AllowPlayerActions)
        {
            for (int i = 1; i <= currentDisplayedItems.Count; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    EquipItem(currentDisplayedItems[i - 1]);
                    break;
                }
            }
        }
    }

    private bool IsOpen => uiMask.activeSelf;

    public void Open()
    {
        GameManager.FreezeCamera();
        uiMask.SetActive(true);
        DisplayAllItems();
    }

    public void Close()
    {
        uiMask.SetActive(false);
        GameManager.UnfreezeCamera();
    }

    private int StartXSlotPosition => INVENTORY_SLOT_SPACE + INVENTORY_SLOT_SIZE / 2;

    private void DisplayAllItems()
    {
        Vector2 size = InventorySize;
        ItemDisplayer.Fill(R, slotPrefab, uiMask.transform, items, slots, 
            INVENTORY_SLOT_SIZE, INVENTORY_SLOT_SPACE, ClickEvent, HoverEvent,HoverExitEvent);
    }


    private void UpdateCurrentDisplayedItems()
    {
        currentDisplayedItems = new List<EquipableItem>();
        int count = 0;
        foreach (EquipableItem e in items.Keys.Select(r => r.RuntimeRef).Where(i => i is EquipableItem))
        {
            currentDisplayedItems.Add(e);
            inventorySprites[count].transform.parent.gameObject.SetActive(true);
            inventorySprites[count].sprite = e.icon;
            count++;
        }
        while (count < inventorySprites.Count)
        {
            inventorySprites[count].transform.parent.gameObject.SetActive(false);
            count++;
        }
    }

    private void ClickEvent(InventoryItemRef r)
    {
        Debug.Log("clicked");
    }

    private void HoverEvent(InventoryItemRef r)
    {
        Debug.Log("hoverenter on " + r.RuntimeRef.itemName);

    }
    
    private void HoverExitEvent(InventoryItemRef r)
    {
        Debug.Log("hoverexit");
    }
    public CursorLockMode CursorMode => CursorLockMode.None;

    public GameObject uiMask;

}
