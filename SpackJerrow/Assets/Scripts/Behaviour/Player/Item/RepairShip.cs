using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairShip : ItemBehaviour
{

    public Vector3 startEuler;

    public Vector3 endEuler;

    public Inventory inventory;

    public InventoryItem wood;
    
    public Transform rayCastPosition;
    
    public float animLength = 0.666f;

    public int repairPower;

    private bool repaired;

    private bool onCooldown;

    private new void Start()
    {
        base.Start();
        if (inventory == null)
        {
            inventory = GameManager.GetPlayerComponent<Inventory>();
        }
    }
    
    private void Update()
    {
        if (GameManager.AllowPlayerActions)
        {
            if (!onCooldown && Input.GetButton("PrimaryAction"))
            {
                onCooldown = true;
                StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(startEuler, endEuler, animLength / 2, v => transform.localEulerAngles = v, () =>
                {
                    StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(endEuler, startEuler, animLength / 2,
                         v => transform.localEulerAngles = v,
                         () =>
                         {
                             onCooldown = false;
                             repaired = false;
                         }
                        ));
                }));
            }
        }
        if(!repaired && onCooldown)
        {
            RaycastHit hit;
            if(Physics.Raycast(rayCastPosition.position,rayCastPosition.forward,out hit, 0.55f))
            {
                Boat boat = hit.collider.gameObject.GetComponentInParent<Boat>();
                if (boat != null && boat.Mass > boat.startMass && inventory.RemoveItem(wood))
                {
                    boat.Mass = Mathf.Max(boat.startMass, boat.Mass - repairPower);
                    repaired = true;
                }
            }
        }
    }

}
