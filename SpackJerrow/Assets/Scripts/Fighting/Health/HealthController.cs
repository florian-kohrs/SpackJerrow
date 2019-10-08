using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthController : RegeneratingValue, IHealthController
{
    public bool IsAlive => CurrentHealth > 0;

    public int CurrentHealth => (int)CurrentValue;

    public abstract void Die();

    public bool IsFullLife => CurrentValue == maxValue;

    public void Heal(int health)
    {
        if (IsAlive)
        {
            Increase(health);
        }
    }

    public bool Damage(int damage)
    {
        if (enabled && IsAlive)
        {
            Reduce(damage);
            bool result = !IsAlive;
            if (result)
            {
                Die();
            }
            return result;
        }
        else
        {
            return false;
        }
    }
}
