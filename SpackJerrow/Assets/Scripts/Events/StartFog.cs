using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFog : SaveableMonoBehaviour
{

    public ParticleSystem particle;

    [Save]
    public bool fogEnabled;

    public void EnableFog()
    {
        fogEnabled = true;
        ActivatePortal();
    }

    private void ActivatePortal()
    {
        particle.Play();
        GetComponent<DistanceTeleport>().Activate();
    }

    protected override void BehaviourLoaded()
    {
        if (fogEnabled)
        {
            ParticleSystem.MainModule main = particle.main;
            main.loop = true;
            main.prewarm = true;
            ActivatePortal();
        }
    }

    protected override void OnFirstTimeBehaviourAwakend()
    {
        particle.Stop();
        particle.Clear();
        GetComponent<DistanceTeleport>().Deactivate();
    }

}
