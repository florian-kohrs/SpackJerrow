using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtension
{

    public static IEnumerator DoDelayed(this MonoBehaviour source, float delay, System.Action action)
    {
        IEnumerator result = DoStuff(delay,action);
        source.StartCoroutine(result);
        return result;
    }

    private static IEnumerator DoStuff(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

}
