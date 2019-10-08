using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTeleport : Teleport
{

    public float radius;
    
    [Save]
    private bool inside;

    Transform player;

    private void Start()
    {
        player = GameManager.Player.transform;
    }

    private float timeAlive = 0;

    void Update()
    {
        if ((transform.position - player.position).sqrMagnitude <= radius * radius)
        {
            if (!inside && timeAlive > portalActivationDelay)
            {
                TeleportPlayer(player);
            }
            inside = true;
        }
        else
        {
            inside = false;
        }
        timeAlive += Time.deltaTime;
    }

}
