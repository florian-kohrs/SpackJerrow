using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateSaveableOnPlay : SaveableMonoBehaviour
{
    protected override void OnFirstTimeBehaviourAwakend()
    {
        gameObject.SetActive(false);
    }
    
}
