using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrade : MonoBehaviour
{

    public Inventory player;

    public Inventory npc;

    public InventoryTrade trade;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            trade.StartTrade(player, npc);
        }
    }
}
