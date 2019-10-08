using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Sword : MonoBehaviour
{

    private Collider hitBox;

    public int damage;

    private void Start()
    {
        if (hitBox == null)
        {
            hitBox = GetComponent<Collider>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            IHealthController target = other.gameObject.GetInterface<IHealthController>();
            if (target != null)
            {
                target.Damage(damage);
                hitBox.enabled = false;
            }
        }
    }

}
