using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : SaveableMonoBehaviour
{

    [Save]
    public string nextSceneName;

    public Vector3 spawnPosition;

    public Vector3 spawnRotation;
    
    public bool isActive;

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }
    
    [Save]
    public bool saveCurrent = true;

    [Save]
    public bool loadNext = true;

    public float portalActivationDelay = 0.34f;

    public override void OnAwake()
    {
        isActive = false;
        StartCoroutine(ActivatePortalDelayed());
    }

    private IEnumerator ActivatePortalDelayed()
    {
        yield return new WaitForSeconds(portalActivationDelay);
        Activate();
    }

    public void TeleportPlayer(Transform player)
    {
        if (isActive)
        {
            SaveablePrefabRoot root = player.parent.GetComponent<SaveablePrefabRoot>();
            SaveablePrefabRoot boat = player.GetComponent<PlayerSailor>().boat?.GetComponent<SaveablePrefabRoot>();
            if (boat != null)
            {
                root.transform.GetChild(0).position = new Vector3(spawnPosition.x, spawnPosition.y + 3, spawnPosition.z); // new Vector3(234, 4, -128);-317 0.5 127.5
                boat.transform.eulerAngles = spawnRotation;// new Vector3(0, 180, 0);
                                                           // boat.getComponent<Boat>().SetSailsTo(0);
                boat.GetComponent<WaterRigidBody>().Velocity = Vector3.zero;
                boat.transform.position = spawnPosition;
                SceneSwitcher.EnterScene(nextSceneName, saveCurrent, loadNext, root, boat);
            }
        }
    }
    
}
