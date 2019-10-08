using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteersmanHealth : HealthController
{
    
    public PirateSwordFight fight;

    public DialogTrigger dialog;

    public Sword sword;

    public override void Die()
    {
        fight.CurrentAnimation.Abort();
        sword.gameObject.SetActive(false);
        fight.AbortAggression();
        dialog.SetDialogProgress(1);
        GameManager.Story.defeatedSteersman = true;
        GetComponent<DialogTrigger>().EnableInteraction();
        GameManager.Story.IsFightingSteersman = false;
        GetComponent<PirateSwordFight>().enabled = false;
    }
    
}
