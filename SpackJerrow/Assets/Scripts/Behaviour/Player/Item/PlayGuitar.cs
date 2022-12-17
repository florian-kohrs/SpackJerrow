using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayGuitar : ItemBehaviour
{

    private PlayerSkillController playerSkill;
    private PlayerSkillController PlayerSkill
    {
        get
        {
            if(playerSkill == null)
                playerSkill = GameManager.GetPlayerComponent<PlayerSkillController>();
            return playerSkill;
        }
    }

    private float pressedDuration;

    public float startSongAfter = 0.3f;

    private bool isPlaying;

    public List<AudioClip> singleTone;

    public List<AudioClipList> songs;

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
                    AudioClipList potentialSongs = songs[PlayerSkill.guitarSkill];
                    audioSource.clip = potentialSongs[Random.Range(0, potentialSongs.Count)];
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
