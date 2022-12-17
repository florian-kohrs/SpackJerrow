using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogNode
{

    string BtnText { get; }

    bool IsLastNode { get; }

    bool IsConnectorNode { get; }

    IDialogNode[] choices { get; }

}
