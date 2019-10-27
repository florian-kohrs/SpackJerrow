using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportActivator : MonoBehaviour
{

    public MeshRenderer teleportMesh;

    public BoxCollider teleportCollider;

    public void Activate()
    {
        teleportCollider.enabled = true;
        teleportMesh.enabled = true;
    }

}
