using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTrade : MonoBehaviour,IInterfaceMask
{

    public GameObject tradeUI;

    public GameObject slotPrefab;

    public CursorLockMode CursorMode => CursorLockMode.None;

    public RectTransform playerUI;

    public RectTransform playerTradeUI;

    public RectTransform npcUI;

    public RectTransform npcTradeUI;

    private List<InventorySlot> playerSlot = new List<InventorySlot>();

    private List<InventorySlot> playerTradeSlot = new List<InventorySlot>();

    private List<InventorySlot> npcSlot = new List<InventorySlot>();

    private List<InventorySlot> npcTradeSlot = new List<InventorySlot>();

    public Text balanceText;

    private int balance;

    private Inventory playerInv;

    private Inventory npcInv;

    public Slider itemCountSlider;
    
    public void StartTrade(Inventory playerInv, Inventory npcInv)
    {
        this.playerInv = playerInv;
        this.npcInv = npcInv;
        playerTradingItems = new Dictionary<InventoryItemRef, int>();
        npcTradingItems = new Dictionary<InventoryItemRef, int>();
        
        InterfaceController.AddMask(this, true);

        UpdateTrade();
    }

    private void UpdateTrade()
    {
        ///show player items
        ItemDisplayer.Fill(
            playerUI.rect,
            slotPrefab,
            playerUI.transform,
            playerInv.items,
            playerSlot,
            UiInventory.INVENTORY_SLOT_SIZE,
            UiInventory.INVENTORY_SLOT_SPACE,
            OnClickPlayerItem,
            (i) => i.RuntimeRef.canBeSoled);

        ///show npc items
        ItemDisplayer.Fill(
            npcUI.rect,
            slotPrefab,
            npcUI.transform,
            npcInv.items,
            npcSlot,
            UiInventory.INVENTORY_SLOT_SIZE,
            UiInventory.INVENTORY_SLOT_SPACE,
            OnClickNpcItem
            );

        ///show player trading items
        ItemDisplayer.Fill(
            playerTradeUI.rect,
            slotPrefab,
            playerTradeUI.transform,
            playerTradingItems,
            playerTradeSlot,
            UiInventory.INVENTORY_SLOT_SIZE,
            UiInventory.INVENTORY_SLOT_SPACE,
            OnClickPlayerTradingItem
            );

        ///show npc trading items
        ItemDisplayer.Fill(
           npcTradeUI.rect,
           slotPrefab,
           npcTradeUI.transform,
           npcTradingItems,
           npcTradeSlot,
           UiInventory.INVENTORY_SLOT_SIZE,
           UiInventory.INVENTORY_SLOT_SPACE,
           OnClickNpcTradingItem
           );

    }

    private Dictionary<InventoryItemRef, int> playerTradingItems;

    private Dictionary<InventoryItemRef, int> npcTradingItems;

    private void OnClickPlayerItem(InventoryItemRef item)
    {
        playerTradingItems.Add(item, playerInv.items[item]);
        playerInv.RemoveItem(item);
        balance += item.RuntimeRef.value;
        balanceText.text = balance.ToString();
        UpdateTrade();
    }

    private void OnClickPlayerTradingItem(InventoryItemRef item)
    {
        playerInv.AddItem(item, playerTradingItems[item]);
        playerTradingItems.Remove(item);
        balance -= item.RuntimeRef.value;
        balanceText.text = balance.ToString();
        UpdateTrade();
    }

    private void OnClickNpcItem(InventoryItemRef item)
    {
        npcTradingItems.Add(item, npcInv.items[item]);
        npcInv.RemoveItem(item);
        balance -= item.RuntimeRef.value;
        balanceText.text = balance.ToString();
        UpdateTrade();
    }

    private void OnClickNpcTradingItem(InventoryItemRef item)
    {
        npcInv.AddItem(item, npcTradingItems[item]);
        npcTradingItems.Remove(item);
        balance += item.RuntimeRef.value;
        balanceText.text = balance.ToString();
        UpdateTrade();
    }

    public void Open()
    {
        tradeUI.SetActive(true);
        GameManager.FreezePlayer();
    }

    public void Close()
    {
        tradeUI.SetActive(false);
        GameManager.UnfreezePlayer();
    }
}
