using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaRegeneration : RegeneratingValue
{
    public bool Reduce(float cost)
    {
        bool result = CurrentValue >= cost;
        if (result)
        {
            CurrentValue -= cost;
        }
        return result;
    }
}
