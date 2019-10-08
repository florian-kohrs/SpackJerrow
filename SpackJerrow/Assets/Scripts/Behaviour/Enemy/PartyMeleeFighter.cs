using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PartyMeleeFighter : PartyAggression
{
    
    [SerializeField]
    private Transform sword;

    [SerializeField]
    private Collider swordHitBox;

    [SerializeField]
    private List<AttackAnimationDeprecated> hitAnimations;

    public AttackAnimationDeprecated CurrentAnimation { get; private set; }

    public float weaponRange;

    public float hitCooldown;

    private bool onCooldown;

    private float minDistanceToTarget = 0.5f;

    [SerializeField]
    protected float moveSpeed;

    protected float chaseRange = 10;

    protected override bool EvaluateAggressionLoss()
    {
        return SqrDistanceToTarget > chaseRange * chaseRange;
    }

    private bool CanAttack => !onCooldown;

    protected virtual void Update()
    {
        if (IsInRange)
        {
            if (!onCooldown)
            {
                Attack();
            }
        }
        MoveTowardsTarget();
    }

    protected void MoveTowardsTarget()
    {
        Vector3 direction = DirectionToTarget;
        if (direction.magnitude > minDistanceToTarget)
        {
            MoveToTarget(direction);
        }
    }

    protected abstract void MoveToTarget(Vector3 direction);

    protected virtual void OnStartAttack(int attackIndex) { }

    protected virtual void OnAttackOver(int attackIndex) { }

    public void Attack()
    {
        onCooldown = true;
        int index;
        AttackAnimationDeprecated attackAnim = Rand.PickOne(hitAnimations, out index);
        CurrentAnimation = attackAnim;
        attackAnim.PlayAnimation(this,sword, delegate { OnAttackOver(index); swordHitBox.enabled = false; });
        this.DoDelayed(attackAnim.colliderActivateDelay, delegate { swordHitBox.enabled = true; });
        this.DoDelayed(hitCooldown, delegate { onCooldown = false; });
        OnStartAttack(index);
    }

    protected bool IsInRange
    {
        get
        {
            return target != null && DistanceToTarget <= weaponRange;
        }
    }

}
