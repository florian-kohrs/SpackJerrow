using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{

    public Transform[] eyes;

    public float standartBlinkCooldown = 3;

    public float blinkCooldownVariance = 2.5f;

    private float currentCooldown;

    public float blinkDuration = 0.3f;

    private float currentBlinkDownTime;

    public Vector3 blinkDownScale = new Vector3(0.1f, 0.0f, 0.1f);

    public Vector3 eyeDefaultScale = new Vector3(0.1f, 0.1f, 0.1f);

    private float NextCoolDown
    {
        get
        {
            return standartBlinkCooldown + 
                Random.Range(-blinkCooldownVariance, blinkCooldownVariance);
        }
    }

    void Start()
    {
        currentCooldown = NextCoolDown;
    }

    private bool isBlinking;
    
    void Update()
    {
        if (GameManager.GameIsNotFrozen)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0 && !isBlinking)
            {
                isBlinking = true;
                currentCooldown = 0;
                SmoothVectorTransformation.StartUnstoppable(this, () => { return eyes.Select(e => e.localScale).ToArray(); },
                    (v) => { foreach (Transform t in eyes) { t.localScale = v; } }, blinkDownScale, blinkDuration / 2);
            }
            else if (currentCooldown <= -blinkDuration / 2)
            {
                SmoothVectorTransformation.StartUnstoppable(this, () => { return eyes.Select(e => e.localScale).ToArray(); },
                    (v) => { foreach (Transform t in eyes) { t.localScale = v; } }, eyeDefaultScale, blinkDuration / 2);
                currentCooldown = NextCoolDown;
                isBlinking = false;
            }
        }
    }

}
