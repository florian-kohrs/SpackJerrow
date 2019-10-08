using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthController
{

    public float pushForce = 5;

    public override void Die()
    {
        Rigidbody r = this.GetOrAddComponent<Rigidbody>();
        r.isKinematic = false;
        r.AddForce((transform.position - GameManager.Player.transform.position).normalized * pushForce, ForceMode.Impulse);
        SaveableScene.SetAsRootTransform(transform);
        CannonShooter c = GetComponent<CannonShooter>();
        c.StopAllCoroutines();
        c.enabled = false;
    }

}
