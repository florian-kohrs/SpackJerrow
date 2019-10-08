using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTargetTrigger<T,J> where T : SetController<J>
{

    public AreaTargetTrigger(J element, float radius) : this(element, radius,180) { }

    public AreaTargetTrigger(J element, float radius, float angle)
    {
        this.element = element;
        this.radius = radius;
        this.angle = angle;
    }

    private J element;

    private Transform target;

    private T controller;
    
    private bool isInside;

    private float radius;

    private float angle;

    public void Remove()
    {
        if (isInside)
        {
            GameManager.GetPlayerComponent<T>()?.RemoveElement(element);
            isInside = false;
        }
    }

    /// <summary>
    /// distsance until the player enters the radius
    /// </summary>
    /// <param name="position"></param>
    /// <param name="distance"></param>
    public float Update(Vector3 position, out float distance)
    {
        target = GameManager.Player.transform;

        Vector3 dir = position - target.position;
        
        distance = dir.magnitude;

        if (distance <= radius
            && (angle == 180 || Vector3.Angle(GameManager.PlayerLookDirection, dir) <= angle))
        {
            if (!isInside)
            {
                isInside = true;
                target.GetComponent<T>().AddElement(element);
            }
        }
        else
        {
            if (isInside)
            {
                isInside = false;
                target.GetComponent<T>().RemoveElement(element);
            }
        }

        distance -= radius;
        return distance;

    }

}
