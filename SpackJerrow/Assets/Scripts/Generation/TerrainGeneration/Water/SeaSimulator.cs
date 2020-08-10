using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaSimulator : MonoBehaviour, IWaterCallbackReceiver
{

    public List<WaterRigidBody> waterObjects;

    public void AddWaterObject(WaterRigidBody waterBody)
    {
        waterObjects.Add(waterBody);
    }

    public void RemoveWaterObject(WaterRigidBody waterBody)
    {
        if (waterObjects != null && waterObjects.Count > 0)
        {
            waterObjects.Remove(waterBody);
        }
    }

    public DefaultWater water;

    private void Start()
    {
        if (water != null)
        {
            water.CallbackReceiver = this;
        }
    }

    private void Awake()
    {
        foreach (WaterRigidBody o in waterObjects)
        {
            o.FloatController = this;
        }
    }

    public void OnWaterChange()
    {
        if (enabled)
        {
            UpdateObjects();
        }
    }

    private void UpdateObjects()
    {
        foreach (WaterRigidBody o in waterObjects)
        {
            o.ApplyFloatInfo(water.GetObjectFloatInfo(o));
            o.OnWaterUpdated();
        }
    }

}
