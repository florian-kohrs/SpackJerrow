using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfiguration : MonoBehaviour
{

    public Button newGameButton;

    public Canvas canvas;

    public Button loadGameButton;

    public Button exitGameButton;
    
    void Start()
    {
        newGameButton.onClick.AddListener(() =>
        {
            Environment.GetWeather().windStrength = 1;
            (Environment.GetSea().waterObjects[0] as Boat).ChangeSailsFor(0.0001f);
            StartCoroutine(StartNewGame());
            canvas.enabled = false;
        });

        loadGameButton.onClick.AddListener(
            ()=> 
            {
                SaveLoad.Load();
            }
        );

        exitGameButton.onClick.AddListener(
            () =>
            {
                Application.Quit();
            }
        );
    }


    private IEnumerator StartNewGame()
    {
        yield return new WaitForSeconds(4);
        PersistentGameDataController.NewGame("MainMap");
    }

}
