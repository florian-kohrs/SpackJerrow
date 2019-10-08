using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTeleport : Teleport
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TeleportPlayer(other.transform);
        }
    }
}
