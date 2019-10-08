using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : SaveableMonoBehaviour
{


    protected override void OnFirstTimeBehaviourAwakend()
    {
        Hide();
    }

    public void Hide()
    {
        transform.Translate(0, -100, 0);
    }

    public void Show()
    {
        transform.Translate(0, 100, 0);
    }

}
