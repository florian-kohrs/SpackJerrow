using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseInteraction : BaseInteraction
{

    public const string OPEN_INTERACTION_BUTTON_NAME = "Open";

    public bool StartOpen;

    public override string InteractionDescription
    {
        get
        {
            if (IsOpen)
            {
                return "Close";
            }
            else
            {
                return "Open";
            }
        }
    }

    [Save]
    private bool isOpen;

    public float transitionSpeed;
    
    private IEnumerator transitionAnimation;

    public Vector3 openLocalEuler;

    public Vector3 closeLocalEuler;

    [Tooltip("This object will be rotated on open and close")]
    public Transform rotatedObject;

    private GameObject interactor;

    private Vector3 NextEuler
    {
        get
        {
            if (IsOpen)
            {
                return closeLocalEuler;
            }
            else
            {
                return openLocalEuler;
            }
        }
    }

    private Vector3 CurrentEuler
    {
        get
        {
            if (!IsOpen)
            {
                return closeLocalEuler;
            }
            else
            {
                return openLocalEuler;
            }
        }
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        isOpen = StartOpen;
    }

    protected override bool CanInteract => true;

    protected new void Start()
    {
        rotatedObject.localEulerAngles = CurrentEuler;
        base.Start();
    }

    public bool IsOpen
    {
        get
        {
            return isOpen;
        }
        private set
        {
            isOpen = value;
            if (isOpen)
            {
                OnOpen(interactor);
            }
            else
            {
                OnClose(interactor);
            }
        }
    }

    protected virtual void OnOpen(GameObject interactor) { }

    protected virtual void OnClose(GameObject interactor) { }

    private void SwapStatus()
    {
        AnimateStatusTransition();
        IsOpen = !IsOpen;
    }

    public override string InteractButtonName => OPEN_INTERACTION_BUTTON_NAME;

    private void SetEulerToStatus()
    {
        if (IsOpen)
        {
            transform.localEulerAngles = openLocalEuler;
        }
        else
        {
            transform.localEulerAngles = closeLocalEuler;
        }
    }

    private void AnimateStatusTransition()
    {
        if(transitionAnimation != null)
        {
            StopCoroutine(transitionAnimation);
        }
        transitionAnimation = SmoothTransformation<Vector3>.
            SmoothRotateAngleEuler(
                rotatedObject.localEulerAngles,
                NextEuler, 
                transitionSpeed,
                v => rotatedObject.localEulerAngles = v
            );
        StartCoroutine(transitionAnimation);
    }

    protected override void Interact_(GameObject interactor)
    {
        this.interactor = interactor;
        SwapStatus();
    }

    
}
