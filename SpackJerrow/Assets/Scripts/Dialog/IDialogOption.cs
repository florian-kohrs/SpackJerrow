using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogOption
{

    DialogAction GetDialogOption();

    bool IsDialogOptionAvailable();

}
