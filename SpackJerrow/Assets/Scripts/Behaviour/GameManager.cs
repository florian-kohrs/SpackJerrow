using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{

    static GameManager()
    {
        SceneManager.activeSceneChanged += delegate
        {
            instance = new GameManager();
        };
    }

    private GameManager()
    {
        Time.timeScale = 1;
    }

    private bool isGameFrozen;

    private bool isMovementBlocked;

    private bool isPlayerMovementBlocked;
    
    private bool isActionBlocked;

    private bool isPlayerActionBlocked;

    private bool isCameraBlocked;

    private GameObject player;

    private Camera playerMainCamera;

    public static Camera PlayerMainCamera
    {
        get
        {
            if(instance.playerMainCamera == null)
            {
                instance.playerMainCamera = Camera.main;
            }
            return instance.playerMainCamera;
        }
        set
        {
            instance.playerMainCamera = value;
        }
    }

    public static bool IsPlayerAlive
    {
        get
        {
            return GetPlayerComponent<HealthController>().IsAlive;
        }
    }
    private static GameManager GM
    {
        get
        {
            if(instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private static GameManager instance;

    public static void ResetPlayerRef()
    {
        GM.player = null;
    }

    public static Vector3 PlayerLookDirection
    {
        get
        {
            return Camera.main.transform.forward;
        }
    }

    public static StoryController Story
    {
        get
        {
            return Player.transform.parent.GetComponent<StoryController>();
        }
    }
    
    public static GameObject Player
    {
        get
        {
            if(GM.player == null)
            {
                GM.player = GameObject.FindGameObjectWithTag("Player");
            }
            return GM.player;
        }
    }

    public static T GetPlayerComponent<T>() where T: Component
    {
        return Player?.GetComponent<T>();
    }

    public static void FreezeCamera()
    {
        GM.isCameraBlocked = true;
    }

    public static void FreezePlayer()
    {
        FreezeCamera();
        DisablePlayerMovement();
        DisablePlayerActions();
    }

    public static void UnfreezePlayer()
    {
        UnfreezeCamera();
        EnablePlayerMovement();
        EnablePlayerActions();
    }

    public static void UnfreezeCamera()
    {
        GM.isCameraBlocked = false;
    }

    public static bool CanCameraMove
    {
        get
        {
            return GameIsNotFrozen && !GM.isCameraBlocked;
        }
    }

    public static void FreezeGame()
    {
        GM.isGameFrozen = true;
        Time.timeScale = 0;
    }

    public static void UnfreezeGame()
    {
        GM.isGameFrozen = false;
        Time.timeScale = 1;
    }

    public static bool GameIsNotFrozen
    {
        get
        {
            return !GM.isGameFrozen;
        }
    }

    public static void DisableMovement()
    {
        GM.isMovementBlocked = true;
    }

    public static void EnableMovement()
    {
        GM.isMovementBlocked = false;
    }

    public static void DisablePlayerMovement()
    {
        GM.isPlayerMovementBlocked = true;
    }

    public static void EnablePlayerMovement()
    {
        GM.isPlayerMovementBlocked = false;
    }

    public static bool AllowMovement
    {
        get
        {
            return GameIsNotFrozen && !GM.isMovementBlocked;
        }
    }

    public static bool AllowPlayerMovement
    {
        get
        {
            return GameIsNotFrozen && !GM.isPlayerMovementBlocked;
        }
    }

    public static void DisableActions()
    {
        GM.isActionBlocked = true;
    }

    public static void EnableActions()
    {
        GM.isActionBlocked = false;
    }

    public static bool AllowActions
    {
        get
        {
            return GameIsNotFrozen && !GM.isActionBlocked;
        }
    }

    public static void DisablePlayerActions()
    {
        GM.isPlayerActionBlocked = true;
    }

    public static void EnablePlayerActions()
    {
        GM.isPlayerActionBlocked = false;
    }

    public static bool AllowPlayerActions
    {
        get
        {
            return GameIsNotFrozen && !GM.isPlayerActionBlocked;
        }
    }

}
