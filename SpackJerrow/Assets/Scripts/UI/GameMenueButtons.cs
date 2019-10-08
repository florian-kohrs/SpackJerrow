using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenueButtons : MonoBehaviour
{

    public GameObject settings;

    public void SaveGame()
    {
        SaveLoad.Save();
    }

    public void LoadGame()
    {
        SaveLoad.Load();
    }

    public void Continue()
    {
        GameManager.GetPlayerComponent<InterfaceController>().RemoveMask();
    }

    public void OpenSettings()
    {
        settings.SetActive(!settings.activeSelf);
    }


    public void SafeAndExit()
    {
        SaveGame();
        Application.Quit();
    }

}
