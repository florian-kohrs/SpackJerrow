using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordFight : ItemBehaviour
{

    public Collider swordHitBox;

    public float attackCooldown;

    private bool isOnCooldown;

    public int damage;

    public float timeToIdle;
    
    private IEnumerator toIdle;

    #region animation

    public Animation defaultSwordHit;

    public Vector3 idleFightPosition;
    public Vector3 idleFightRotation;

    public Vector3 defaultLocalPosition;
    public Vector3 defaulLocalEuler;

    public float animToIdleTime;

    private IEnumerator SetIdleDelayed()
    {
        yield return new WaitForSeconds(timeToIdle);
        Animation.PlayTransitionTo(this, defaultLocalPosition, defaulLocalEuler, animToIdleTime);
    }

    private IEnumerator EnableHitboxDelayed()
    {
        yield return new WaitForSeconds(defaultSwordHit.animations[0].timeNeeded);
        swordHitBox.enabled = true;
    }

    private void PlayHitAnim()
    {
        if(toIdle != null)
        {
            StopCoroutine(toIdle);
        }
        StartCoroutine(EnableHitboxDelayed());
        defaultSwordHit.PlayAnimation(this, () => { swordHitBox.enabled = false; toIdle = SetIdleDelayed(); StartCoroutine(toIdle); });
    }

    #endregion

    private new void Start()
    {
        base.Start();
        defaultLocalPosition = transform.localPosition;
        defaulLocalEuler = transform.localEulerAngles;
    }

    private void Update()
    {
        if (GameManager.AllowPlayerActions)
        {
            if (!isOnCooldown && Input.GetButtonDown("PrimaryAction"))
            {
                isOnCooldown = true;
                StartCoroutine(ResetCooldown());
                PlayHitAnim();
            }
        }
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            IHealthController target = other.gameObject.GetInterface<IHealthController>();
            if (target != null)
            {
                target.Damage(damage);
                swordHitBox.enabled = false;
            }
        }
    }

}
