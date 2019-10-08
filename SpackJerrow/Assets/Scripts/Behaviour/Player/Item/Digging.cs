using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Digging : ItemBehaviour
{

    public List<AudioClip> succesDigSounds;

    public List<AudioClip> wrongSurfaceDiggingSound;

    public List<AudioClip> missedDigSound;

    public AudioSource sourceDiggingSounds;
    
    //public AudioSource sourceCreatureSounds;
    
    public float digCooldown = 0.75f;

    public int digSize = 6;

    private bool onCooldown;
    public bool fireRayFromParent;

    private Vector3 digStartRotation = new Vector3(-47.313f, -19.313f, -10.4f);

    private Vector3 endAnimPosition = new Vector3(0.378f, -0.129f, 0.305f);
    private Vector3 endAnimEuler = new Vector3(-20.1f, 120.7f, -155.8f);

    private Vector3 defaultPosition;
    private Vector3 defaultEuler;
    
    private new void Start()
    {
        base.Start();
        if (sourceDiggingSounds == null)
        {
            sourceDiggingSounds = GetComponent<AudioSource>();
        }
        defaultPosition = transform.localPosition;
        defaultEuler = transform.localEulerAngles;
    }

    private void Update()
    {
        if (!onCooldown && GameManager.AllowPlayerActions && Input.GetButtonDown("PrimaryAction"))
        {
            //Physics.CheckSphere
            RaycastHit hit;
            Vector3 firePosition;
            if (fireRayFromParent)
            {
                firePosition = transform.parent.position;
            }
            else
            {
                firePosition = transform.position;
            }
            ///spherecast?
            if(Physics.Raycast(firePosition, Vector3.up * -1, out hit, 0.75f))
            {
                Island island = hit.transform.GetComponent<Island>();
                if(island != null)
                {
                    Vector3 playerLookDirection = transform.up * -1;
                   // Vector3 playerLookDirection = transform.parent.right;
                    playerLookDirection.y = 0;
                    playerLookDirection.Normalize();
                    island.Dig(transform.position,digSize, 0.5f, 2, 2.5f, 3f, playerLookDirection);
                    PlayRandomClip(sourceDiggingSounds, succesDigSounds);
                    StartDigAnim();
                }
                else
                {
                    StartWrongTerrainDigAnim();
                    PlayRandomClip(sourceDiggingSounds, wrongSurfaceDiggingSound);
                }
            }
            else
            {
                StartDigAnim();
                //PlayRandomClip(sourceDiggingSounds, succesDigSounds);
            }
            onCooldown = true;
            StartCoroutine(DelayNextUse());
        }
    }

    private void StartDigAnim()
    {
        StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(defaultEuler, digStartRotation, 0.0f, (v) => transform.localEulerAngles = v, () =>
        {
            StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(defaultPosition, endAnimPosition, 0.4f,
                 v => transform.localPosition = v,
                 () =>
                 {
                     StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(digStartRotation, endAnimEuler, 0.25f,
                          v => transform.localEulerAngles = v,
                          () => { transform.localPosition = defaultPosition; transform.localEulerAngles = defaultEuler; }));
                 }
                ));
        }));
    }

    private void StartWrongTerrainDigAnim()
    {
        StartCoroutine(SmoothTransformation<Vector3>.GetStoppable(defaultEuler, digStartRotation, 0.0f, (v) => transform.localEulerAngles = v, (s, t, p) => s + (t - s) * p, () =>
        {
            StartCoroutine(SmoothTransformation<Vector3>.GetStoppable(defaultPosition, endAnimPosition, 0.05f,
                 (v) => transform.localPosition = v,
                 (s, t, p) => s + (t - s) * p,
                 () =>
                 {
                   transform.localPosition = defaultPosition; transform.localEulerAngles = defaultEuler;
                 }
                ));
        }));
     }

    private IEnumerator DelayNextUse()
    {
        yield return new WaitForSeconds(digCooldown);
        onCooldown = false;
    }

    private void PlayRandomClip(AudioSource source, List<AudioClip> clips)
    {
        //source.clip = clips[Random.Range(0, clips.Count)];
        //source.Play();
        source.PlayOneShot(clips[Random.Range(0, clips.Count)]);
    }

}
