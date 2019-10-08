using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGlobalSounds : MonoBehaviour
{

    public AudioSource source;

    private void Start()
    {
        source.spatialBlend = 0;
    }
    
    public static void PlayClip(AudioClip clip)
    {
        GameManager.GetPlayerComponent<PlayerGlobalSounds>().source.PlayOneShot(clip);
    }

    public static void PlayClip(AudioClip clip, float volume)
    {
        GameManager.GetPlayerComponent<PlayerGlobalSounds>().source.PlayOneShot(clip,volume);
    }

}
