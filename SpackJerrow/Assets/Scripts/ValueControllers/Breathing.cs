using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breathing : RegeneratingValue
{

    public Image breathImage;

    public GameObject endScreenContainer;

    private const string DROWN_MESSAGE = "You drowned!";

    public Text drownText;

    private bool drowned;

    [Save]
    private bool canBreath = true;

    public bool CanBreath
    {
        get
        {
            return canBreath;
        }
        set
        {
            canBreath = value;
        }
    }

    protected override void Update()
    {
        if (canBreath)
        {
            base.Update();
        }
        else
        {
            CurrentValue -= Time.deltaTime;
        }
    }

    public virtual void Drowned()
    {
        drownText.text = DROWN_MESSAGE;
        endScreenContainer.SetActive(true);
        drowned = true;
        GameManager.FreezeGame();
        MouseLocker.FreeCursor();
    }

    protected override void OnValueChanged(float delta)
    {
        Color c = breathImage.color;
        float breathLeft = Mathf.InverseLerp(0, maxValue, CurrentValue);
        float linearAlpha = (1 - breathLeft) / 2;
        if (delta < 0)
        {
            float cosAlpha = Mathf.Cos(Mathf.PI * breathLeft * 6.5f) / 2;
            c.a = Mathf.Abs(cosAlpha/*,0 , 0.5f*/) + linearAlpha;
        }
        else
        {
            c.a = linearAlpha;
        }

        if(CurrentValue <= 0)
        {
            if (!drowned)
            {
                Drowned();
            }
        }

        breathImage.color = c;
    }

}
