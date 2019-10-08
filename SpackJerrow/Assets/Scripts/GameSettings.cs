using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{

    public Slider waterSizeSlider;

    public Text waterSizeInfoText;

    public Text colorblindText;

    public Toggle colorblindToggle;

    public Toggle fpsToggle;

    public Text fpsDisplayer;

    public Text fpsInfoText;

    private Settings s;

    private Settings S
    {
        get
        {
            if (s == null)
            {
                s = PersistentGameDataController.Settings;
            }
            return s;
        }
    }
    
    private void Awake()
    {
        
        waterSizeSlider.minValue = minWaterSize;
        waterSizeSlider.maxValue = maxWaterSize;
        waterSizeSlider.value = S.waterCalculationSize;
        CalculatedWaterSizeChange(S.waterCalculationSize);

        colorblindToggle.isOn = S.isColorBlindModeOn;
        if (S.isColorBlindModeOn)
        {
            colorblindText.text = "On";
        }
        else
        {
            colorblindText.text = "Off";
        }
        
        fpsToggle.isOn = S.displayFps;
        UpdateFpsText();

        cameraSentivitySlider.minValue = minCamSens;
        cameraSentivitySlider.maxValue = maxCamSens;
        cameraSentivitySlider.value = S.cameraSensitivity;
        OnCameraSensitivityChange(S.cameraSensitivity);
        
    }

    private void UpdateFpsText()
    {
        if (fpsDisplayer != null)
        {
            if (S.displayFps)
            {
                fpsInfoText.text = "On";
            }
            else
            {
                fpsInfoText.text = "Off";
            }
            fpsDisplayer.enabled = S.displayFps;
        }
    }

    
    [SerializeField]
    private int minWaterSize = 25;

    [SerializeField]
    private int maxWaterSize = 150;
    
    public void CalculatedWaterSizeChange(float v)
    {
        S.waterCalculationSize = (int)v;
        waterSizeInfoText.text = v + "/" + maxWaterSize;
    }

    public void ColorblindModeChange(bool b)
    {
        S.isColorBlindModeOn = b;
        if (b)
        {
            colorblindText.text = "On (Save and load to apply change)";
        }
        else
        {
            colorblindText.text = "Off (Save and load to apply change)";
        }
    }

    public void OnFpsToggleChange(bool b)
    {
        S.displayFps = b;
        UpdateFpsText();
    }

    public Slider cameraSentivitySlider;

    public Text camSenInfo;


    [SerializeField]
    private int minCamSens = 5;

    [SerializeField]
    private int maxCamSens = 200;

    public void OnCameraSensitivityChange(float v)
    {
        S.cameraSensitivity = v;
        camSenInfo.text = v + "/" + maxCamSens;
    }

    public void SaveSettings()
    {
        PersistentGameDataController.SaveSettings();
    }

    private void OnDestroy()
    {
        SaveSettings();
    }

}
