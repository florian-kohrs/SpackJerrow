using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blend : MonoBehaviour
{

    public Image image;

    public float blendTime;

    public float blendProgress;
    
    private void Update()
    {
        if (GameManager.GameIsNotFrozen)
        {
            Color newColor = image.color;
            newColor.a = (blendTime - blendProgress) / blendTime;
            image.color = newColor;
            if (blendProgress >= blendTime)
            {
                enabled = false;
            }
            else
            {
                blendProgress += Time.deltaTime;
                //if (currentBlendTime > blendTime)
                //{
                //    currentBlendTime = blendTime;
                //}
            }
        }
    }

}
