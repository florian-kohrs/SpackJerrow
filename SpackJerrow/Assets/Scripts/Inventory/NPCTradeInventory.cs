using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTradeInventory : NPCInventory, IDialogOption
{
    
    public bool IsDialogOptionAvailable() => true;

    public DialogAction GetDialogOption()
    {
        throw new NotImplementedException();
    }

    private void OpenTradeMenue()
    {

    }
}
