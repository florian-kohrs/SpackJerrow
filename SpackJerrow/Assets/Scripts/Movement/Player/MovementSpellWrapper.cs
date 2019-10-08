using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementSpellWrapper
{

    public MovementSpell movement;

    private bool onCooldown = false;

    /// <summary>
    /// checks if the player triggered the expected input
    /// </summary>
    /// <returns></returns>
    public bool IsCasting(IMovementWizard wizard)
    {
        ///is the right input pressed and not on cooldown
        return movement != null && !onCooldown && (movement.canBeCastInAir || !wizard.IsWizardFlying) && Evaluator.Invoke(movement.buttonTriggerName);
    }

    private Func<string, bool> evaluator;

    private Func<string, bool> Evaluator
    {
        get
        {
            if(evaluator == null)
            {
                evaluator = GetEvaluator();
            }
            return evaluator;
        }
    }

    public void Cast(IMovementWizard wizard)
    {
        onCooldown = true;
        Vector3 direction = wizard.Direction(movement.direction);

        if (movement.normalizeDirection)
        {
            direction.Normalize();
        }

        direction *= movement.power;

        if (movement.useDeltaTime)
        {
            direction *= Time.deltaTime;
        }

        wizard.Move(direction,movement.forceMode);
        wizard.Behaviour.StartCoroutine(DelayNextCast());
    }

    private IEnumerator DelayNextCast()
    {
        yield return new WaitForSeconds(movement.cooldown);
        onCooldown = false;
    }

    private Func<string,bool> GetEvaluator()
    {
        Func<string, bool> result;
        switch (movement.input)
        {
            case (InputTrigger.OnDown):
                {
                    result = Input.GetButtonDown;
                    break;
                }
            case (InputTrigger.Pressed):
                {
                    result = Input.GetButton;
                    break;
                }
            case (InputTrigger.Up):
                {
                    result = Input.GetButtonUp;
                    break;
                }
            case (InputTrigger.Permanent):
                {
                    result = _ => true;
                    break;
                }
            default:
                {
                    result = null;
                    Debug.LogWarning("Unknown Input trigger used: " + movement.input.ToString());
                    break;
                }
        }
        return result;
    }
    

}
