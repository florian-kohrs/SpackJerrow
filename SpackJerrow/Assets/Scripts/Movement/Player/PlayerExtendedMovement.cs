using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StaminaRegeneration))]
public class PlayerExtendedMovement : PlayerMovementController, IMovementWizard
{

    public override Vector3 InputForce
    {
        get
        {
            return body.velocity;
        }
    }

    private Vector3 currentInputForce;

    public Transform cameraTransform;

    public List<MovementSpellWrapper> landMovement;

    public List<MovementSpellWrapper> waterMovement;

    private List<MovementSpellWrapper> usedMovement;

    public void SetMovementSpellList(List<MovementSpellWrapper> spells)
    {
        usedMovement = spells;
    }

    public StaminaRegeneration stamina;

    private bool isInAir;

    protected void Start()
    {
        if (usedMovement == null)
        {
            usedMovement = landMovement;
        }
        if (stamina == null)
        {
            stamina = GetComponent<StaminaRegeneration>();
        }
    }

    private void Update()
    {
        if (GameManager.AllowPlayerMovement)
        {
            CheckMovementSpells();
        }
    }

    private void FixedUpdate()
    {
        isInAir = !Physics.Raycast(transform.position, Vector3.up * -1, 1f);
    }

    public override Transform DirectionTransform()
    {
        return cameraTransform;
    }

    private void CheckMovementSpells()
    {
        foreach(MovementSpellWrapper s in usedMovement)
        {
            if (s.IsCasting(this) && stamina.Reduce(s.movement.cost))
            {
                s.Cast(this);
            }
        } 
    }
    
    public Vector3 Direction(DirectionMode directionMode)
    {
        Vector3 result;

        switch (directionMode)
        {
            case (DirectionMode.Up):
                {
                    result = transform.up;
                    break;
                }
            case (DirectionMode.WorldUp):
                {
                    result = Vector3.up;
                    break;
                }
            case (DirectionMode.Down):
                {
                    result = transform.up * -1;
                    break;
                }
            case (DirectionMode.WorldDown):
                {
                    result = Vector3.up * -1;
                    break;
                }
            case (DirectionMode.TransformedInputAxis2D):
                {
                    result = cameraTransform.TransformDirection(CurrentInputDirection);
                    result.y = 0;
                    result.Normalize();
                    break;
                }
            case (DirectionMode.Forward):
                {
                    result = transform.forward;
                    break;
                }
            case (DirectionMode.LookDirectionForward):
                {
                    result = cameraTransform.forward;
                    break;
                }
            case (DirectionMode.Back):
                {
                    result = transform.forward * -1;
                    break;
                }
            case (DirectionMode.LookDirectionBack):
                {
                    result = cameraTransform.forward * -1;
                    break;
                }
            case (DirectionMode.Right):
                {
                    result = transform.right;
                    break;
                }
            case (DirectionMode.LookDirectionRight):
                {
                    result = cameraTransform.right;
                    break;
                }
            case (DirectionMode.Left):
                {
                    result = transform.right * -1;
                    break;
                }
            case (DirectionMode.LookDirectionLeft):
                {
                    result = cameraTransform.right * -1;
                    break;
                }
            case (DirectionMode.InputAxis):
                {
                    result = CurrentInputDirection;
                    break;
                }
            case (DirectionMode.TransformedInputAxis):
                {
                    result = cameraTransform.TransformDirection(CurrentInputDirection);
                    break;
                }
            default:
                {
                    result = Vector3.zero;
                    Debug.LogWarning("Unknown Direction mode: " + directionMode);
                    break;
                }
        }
        return result;
    }

    public void Move(Vector3 dir, ForceMode forceMode)
    {
        body.AddForce(dir, forceMode);
    }

    public MonoBehaviour Behaviour
    {
        get
        {
            return this;
        }
    }

    public Vector3 CurrentEulerAngle
    {
        get
        {
            return cameraTransform.eulerAngles;
        }
    }

    public override Vector3 LookDiretionInput => cameraTransform.TransformDirection(CurrentInputDirection);

    public bool IsWizardFlying
    {
        get
        {
            return isInAir;
        }

    }
}
