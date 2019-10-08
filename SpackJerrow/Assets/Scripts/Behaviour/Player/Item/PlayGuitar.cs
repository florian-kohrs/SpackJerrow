using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayGuitar : ItemBehaviour
{

    private float pressedDuration;

    public float startSongAfter = 0.3f;

    private bool isPlaying;

    public List<AudioClip> singleTone;

    public List<AudioClip> songs;

    public AudioSource audioSource;

    protected new void Start()
    {
        base.Start();
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (GameManager.AllowPlayerActions)
        {
            if (Input.GetButton("PrimaryAction"))
            {
                pressedDuration += Time.deltaTime;
                if (pressedDuration >= startSongAfter && !isPlaying)
                {
                    audioSource.clip = songs[Random.Range(0, songs.Count)];
                    audioSource.Play();
                    isPlaying = true;
                }
            }
            else if (Input.GetButtonUp("PrimaryAction"))
            {
                if (pressedDuration < startSongAfter)
                {
                    audioSource.PlayOneShot(singleTone[Random.Range(0, singleTone.Count)]);
                }
                else
                {
                    audioSource.Stop();
                }
                pressedDuration = 0;
                isPlaying = false;
            }
        }
    }

}
