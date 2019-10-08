using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMusic : MonoBehaviour
{

    public AudioClip clip;
    
    public void PlayMusic()
    {
        PlayerGlobalSounds.PlayClip(clip,0.55f);
    }

}
