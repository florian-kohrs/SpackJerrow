using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogTrigger : BaseInteraction
{

    public string dialogDudeName = "Unimportant Character";

    private const string START_DIALOG_BUTTON = "Talk";

    public override string InteractButtonName => START_DIALOG_BUTTON;

    public override string InteractionDescription => "Talk to " + dialogDudeName;

    public override float InteractionRange => 5;
    
    public List<DialogNode> nodes;
    
    private AudioSource source;

    [Save]
    private int dialogProgress;

    public void IncreaseDialogProgress(int increment)
    {
        dialogProgress += increment;
    }

    public void SetDialogProgress(int progress)
    {
        dialogProgress = progress;
    }

    public DialogNode CurrentNode => nodes[dialogProgress];

    public AudioSource Source
    {
        get
        {
            if(source == null)
            {
                source = GetComponent<AudioSource>();
            }
            return source;
        }
    }
    
    protected override void Interact_(GameObject interactor)
    {
        DialogSystem dialogSystem = interactor.GetComponent<DialogSystem>();
        dialogSystem.StartDialog(this);
    }

}
