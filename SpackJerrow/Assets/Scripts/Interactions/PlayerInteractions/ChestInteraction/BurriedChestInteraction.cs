using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurriedChestInteraction : LootChestInteraction
{

    public Transform[] checkForSurface;

    //private List<int> alreadySurfaced = new List<int>();

    public float surfacedValueToBeOpened = 0.6f;

    [Save]
    public IHeightInfo surface;

    [Save]   
    private bool canBeOpened = false;

    [Tooltip("If this is true, the surfaced this chest is burried under will mark the chest")]
    [Save]
    public bool isMarked;

    protected override bool CanInteract
    {
        get
        {
            bool result = surface != null;
            if (result)
            {
                if (!canBeOpened)
                {
                    CheckForSurface();
                }
                result = canBeOpened;
            }
            return result;
        }
    }
    
    private void CheckForSurface()
    {
        int surfacePoints = 0;
        canBeOpened = false;
        for(int i = 0; i < checkForSurface.Length && !canBeOpened; i++)
        {
            Vector3 point = checkForSurface[i].position;
            IndexInfo info = surface.GetNearestIndexInfo(surface.GlobalPosToProgress(new Vector2(point.x, point.z)));
            float height = surface.AbsoluteHeightOnNearestIndex(new Vector2(point.x, point.z));
            if (point.y > height) 
            {
                //if(hit.transform == transform)
                //{
                    surfacePoints++;
                    canBeOpened = (float)surfacePoints / checkForSurface.Length >= surfacedValueToBeOpened;
                // }
            }
        }
        //Debug.Log("Chest surface percentage is: " + (float)surfacePoints / checkForSurface.Length);
    }

    //private void OnDrawGizmos()
    //{
    //    int surfacePoints = 0;
    //    canBeOpened = false;
    //    for (int i = 0; i < checkForSurface.Length && !canBeOpened; i++)
    //    {
    //        Vector3 point = checkForSurface[i].position;
    //        IndexInfo info = surface.GetNearestIndexInfo(surface.ToLocalProgress(new Vector2(point.x, point.z)));
    //        float height = surface.HeightOnNearestIndex(new Vector2(point.x, point.z));
    //        if (point.y > height)
    //        {
    //            //if(hit.transform == transform)
    //            //{
    //            surfacePoints++;
    //            canBeOpened = (float)surfacePoints / checkForSurface.Length >= surfacedValueToBeOpened;
    //            // }
    //        }
        
    //    Gizmos.DrawSphere(new Vector3(point.x, height, point.z), 0.1f);
    //    }
    //    //Debug.Log("Chest surface percentage is: " + (float)surfacePoints / checkForSurface.Length);
    //}

}
