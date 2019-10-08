using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteersmanToGovernor : SaveableMonoBehaviour
{

    [Save]
    public bool fired;
    
    public DialogTrigger dialog;

    private void OnTriggerEnter(Collider other)
    {
        if(!fired && other.tag == "Steersman" && GameManager.Story.defeatedSteersman)
        {
            Transform cell = GameObject.FindGameObjectWithTag("Cell").transform;
            cell.GetComponent<HideOnPlay>().Show();
            other.GetComponent<DialogTrigger>().DisableInteraction();
            other.GetComponent<MoveTowards>().enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            //  other.transform.LookAt()
            other.transform.parent = cell;
            other.transform.LookAt(transform);
            other.transform.Rotate(0, 180, 0);
            other.transform.localPosition = new Vector3(0.2f, 1.75f, -1.2f);
            dialog.SetDialogProgress(2);
            fired = true;
        }
    }

}
