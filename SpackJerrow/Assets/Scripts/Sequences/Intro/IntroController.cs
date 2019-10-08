using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : SequenceController
{
    
    public AudioSource source;

    public AudioSource buzzSource;

    public AudioClip buzzClip;
    
    public float silenceAfterSeconds = 2f;
    
    public InterfaceController interfaceController;

    [Save]
    public bool playedIntro;
    
    public override void IsFinished()
    {
        if(interfaceController != null)
        {
            interfaceController.enabled = true;
        }
        GameManager.UnfreezePlayer();
        GameManager.GetPlayerComponent<Breathing>().StartCoroutine(LowerSound(silenceAfterSeconds));
    }

    private IEnumerator LowerSound(float timeToZero)
    {
        float timeLeft = timeToZero;
        while(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            source.volume = timeLeft / timeToZero;
            yield return null;
        }
        StartBuzzing();
        gameObject.SetActive(false);
    }

    private void StartBuzzing()
    {
        if (buzzSource != null)
        {
            buzzSource.clip = buzzClip;
            buzzSource.Play();
        }
    }

    protected override void Starting()
    {
        if (interfaceController != null)
        {
            interfaceController.enabled = false;
        }
        playedIntro = true;
        source.Play();
        GameManager.FreezePlayer();
    }

}
