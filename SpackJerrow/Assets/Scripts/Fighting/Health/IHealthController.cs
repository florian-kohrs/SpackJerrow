using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthController
{

    /// <summary>
    /// returns true when the target died due to this attack
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    bool Damage(int damage);
    
    bool IsAlive { get; }
    
    int CurrentHealth { get; }
}
