using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthController
{

    public const string DEATH_TEXT = "You died";

    public Text deathText;

    public GameObject deathImage;

    public Text lifeInfo;

    public Image lifeBar;

    protected override void BehaviourLoaded()
    {
        UpdateView();
    }

    protected override void OnValueChanged(float delta)
    {
        UpdateView();
    }
    
    private void UpdateView()
    {
        lifeInfo.text = (int)CurrentHealth + "/" + (int)maxValue;
        lifeBar.fillAmount = CurrentHealth / maxValue;
    }

    public override void Die()
    {
        deathText.text = DEATH_TEXT;
        deathImage.SetActive(true);
        MouseLocker.FreeCursor();
        Debug.Log("Player Died");
    }

}
