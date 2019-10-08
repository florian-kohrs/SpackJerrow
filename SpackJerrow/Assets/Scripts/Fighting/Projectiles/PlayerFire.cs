using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{

    public float cooldown = 1;

    public bool isOnCooldown;

    public CannonBallDispenser dispenser;

    private void OnCollisionEnter(Collision collision)
    {
        if(!isOnCooldown && collision.gameObject.tag == "Player")
        {
            dispenser.Fire();
            isOnCooldown = true;
            this.DoDelayed(cooldown, delegate { isOnCooldown = false; });
        }
    }

}
