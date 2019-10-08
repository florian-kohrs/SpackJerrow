using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RespawnButton : MonoBehaviour
{

    private void Awake()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() =>
        {
            GameManager.UnfreezeGame();
            SaveLoad.Load();
        });
    }

}
