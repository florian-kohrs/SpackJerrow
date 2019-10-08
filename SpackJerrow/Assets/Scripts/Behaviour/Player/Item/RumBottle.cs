using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RumBottle : ItemBehaviour
{

    public InventoryItemRef item;

    protected Inventory inventory;

    protected PlayerHealth health;

    [SerializeField]
    private SceneAnimation drinkAnimation;

    protected Inventory Inventory
    {
        get
        {
            if (inventory == null)
            {
                inventory = GameManager.GetPlayerComponent<Inventory>();
            }
            return inventory;
        }
    }

    protected PlayerHealth Health
    {
        get
        {
            if (health == null)
            {
                health = GameManager.GetPlayerComponent<PlayerHealth>();
            }
            return health;
        }
    }
    public int healthRestorage = 5;

    public AudioClip gluckGluckSound;

    public AudioClip[] burps;

    void Update()
    {
        if (Input.GetButtonDown("PrimaryAction") && GameManager.AllowPlayerActions && !Health.IsFullLife)
        {
            PlayerGlobalSounds.PlayClip(gluckGluckSound);
            drinkAnimation.PlayAnimation(this, delegate
             {
                 PlayerGlobalSounds.PlayClip(Rand.PickOne(burps));
                 Health.Heal(healthRestorage);
                 Inventory.RemoveItem(item, 1);
             });
        }
    }

}
