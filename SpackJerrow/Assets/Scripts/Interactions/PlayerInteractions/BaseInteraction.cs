using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteraction : SaveableMonoBehaviour
{

    public const float DEFAULT_INTERACTION_RANGE = 3;

    public const string DEFAULT_INTERACTION_BUTTON_NAME = "Interact";

    public const string DEFAULT_INTERACTION_DESCRIPTION = "Interact";

    public const float DEFAULT_INTERACTION_ANGLE = 55;

    private AreaTargetTrigger<InteractionController, BaseInteraction> area;

    public void DisableInteraction()
    {
        isInteractionEnabled = false;
        AbortInteraction();
    }


    internal void DisableInteractionTemp()
    {
        isInteractionTempDisabled = true;
        AbortInteraction();
    }

    public void EnableInteractionTemp()
    {
        isInteractionTempDisabled = false;
    }

    public void EnableInteraction()
    {
        isInteractionTempDisabled = false;
        isInteractionEnabled = true;
    }

    [Save]
    public bool isInteractionEnabled = true;

    public bool isInteractionTempDisabled = false;

    protected AreaTargetTrigger<InteractionController, BaseInteraction> Area
    {
        get
        {
            if(area == null)
            {
                area = new AreaTargetTrigger<InteractionController, BaseInteraction>
                    (this, InteractionRange, InteractionAngle);
            }
            return area;
        }
    }

    protected void Start()
    {
        StartCoroutine(AreaChecker());
    }

    private IEnumerator AreaChecker()
    {
        while (!interactionDestroyed)
        {
            float distance;
            if (CanInteract && Area.Update(transform.position, out distance) < InteractionRange)
            {
                float idleSeconds = Mathf.Clamp(distance / 16, 0.1f, 1f);
                yield return new WaitForSeconds(idleSeconds);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        Area.Remove();
    }

    private bool interactionDestroyed;

    protected override void onDestroy()
    {
        Area.Remove();
    }

    protected void DestroyInteraction()
    {
        interactionDestroyed = true;
    }
        
    public const KeyCode DEFAULT_INTERACTION_KEY_CODE = KeyCode.F;

    public virtual string InteractButtonName => DEFAULT_INTERACTION_BUTTON_NAME;

    public virtual KeyCode InteractKeyCode => DEFAULT_INTERACTION_KEY_CODE;
    
    protected abstract void Interact_(GameObject interactor);

    public void Interact(GameObject interactor)
    {
        Interact_(interactor);
        //Area.Remove();
    }

    public void AbortInteraction()
    {
        Area.Remove();
    }

    protected virtual bool CanInteract
    {
        get
        {
            return canInteract && isInteractionEnabled && !isInteractionTempDisabled;
        }
        private set
        {
            canInteract = value;
        }
    }

    private bool canInteract = true;

    //public abstract bool CanInteract();

    public virtual float InteractionRange => DEFAULT_INTERACTION_RANGE;

    public virtual string InteractionDescription => DEFAULT_INTERACTION_DESCRIPTION;

    public virtual float InteractionAngle => DEFAULT_INTERACTION_ANGLE;

}
