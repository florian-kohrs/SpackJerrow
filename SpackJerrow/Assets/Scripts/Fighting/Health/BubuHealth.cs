using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubuHealth : HealthController
{

    public float knockbackPower = 6f;

    public override void Die()
    {
        GameManager.GetPlayerComponent<UiInventory>().AddItem(schnuckidipuz, 1);
        AnimateItem.AnimateItems(new List<ItemContainer>() { new ItemContainer() { item = schnuckidipuz, itemCount = 1 } }, 0, GameManager.GetPlayerComponent<MonoBehaviour>());
        GetComponent<BubuSwordFight>().enabled = false;
        Rigidbody r = GetComponent<Rigidbody>();
        r.isKinematic = false;
        r.AddForce((transform.position - GameManager.Player.transform.position) * knockbackPower, ForceMode.Impulse);
    }

    public InventoryItemRef schnuckidipuz;

}
