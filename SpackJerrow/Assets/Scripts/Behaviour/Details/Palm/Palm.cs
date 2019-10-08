using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Palm : SaveableMonoBehaviour
{

    private int GetPalmHeight()
    {
        int count = 1;
        Transform child = transform.GetChild(0);
        PalmCutter palmPart = null;
        while (child.childCount > 0)
        {
            child = child.GetChild(0);
            palmPart = child.GetComponent<PalmCutter>();
            if (palmPart != null && palmPart.IsAlive)
            {
                count++;
            }
            else if (palmPart == null && child.GetComponent<Collider>() == null)
            {
                count = -1;
            }
            else
            {
                break;
            }

        }
        return count;
    }

    protected override void setDataBeforeSaving(PersistentGameDataController.SaveType saveType)
    {
        palmHeigth = GetPalmHeight();
    }

    [Save]
    private int palmHeigth = -1;

    protected override void BehaviourLoaded()
    {
        CutPalmAt(palmHeigth);
    }

    private void CutPalmAt(int height)
    {
        if (height >= 0)
        {
            Transform current = transform;
            do
            {
                current = current.GetChild(0);
                height--;
            } while (height >= 0);
            Destroy(current.gameObject);
        }
    }


}
