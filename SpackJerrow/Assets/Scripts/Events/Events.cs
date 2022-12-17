using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events
{

    static Events()
    {
        SceneManager.activeSceneChanged += delegate
        {
            uniqueSceneObject.Clear();
            findables = null;
        };
    }

    /// <summary>
    /// stores for each tag the corresponding gameobject in the scene
    /// </summary>
    private static Dictionary<string, GameObject> uniqueSceneObject = new Dictionary<string, GameObject>();

    private const string FINDABLE_OBJECTS_TAG = "Findables";

    #region const strings/namings

    #region findables names

    private const string MAP_ONE_MAINISLAND_NAME = "MainIsland";

    #region stegs

    private const string MAP_ONE_MAINISLAND_LANDINGSTAGE = "WaitPositionMainLandingStage";

    #endregion

    #endregion

    #region important tag names

    public const string MAP_ONE_SIMPLE_PORTAL_TAG_NAME = "Portal";

    public const string MAP_ONE_PERMUDA_PORTAL_TAG_NAME = "PermudaTri";

    #endregion

    #endregion

    private static GameObject[] findables;

    private static GameObject[] Findables
    {
        get
        {
            if (findables == null)
            {
                findables = GameObject.FindGameObjectsWithTag(FINDABLE_OBJECTS_TAG);
            }
            return findables;
        }
    }

    private static GameObject FindGameObjectWithName(string name)
    {
        return Findables.Where(g => g.name == name).FirstOrDefault();
    }

    private static GameObject FindGameObjectWithTag(string tag)
    {
        GameObject result;
        if (!uniqueSceneObject.TryGetValue(tag, out result))
        {
            result = GameObject.FindGameObjectWithTag(tag);
            if (result != null)
            {
                uniqueSceneObject.Add(tag, result);
            }
        }
        return result;
    }

    public static void RestoreHealth(int value)
    {
        // GameManager.GetPlayerComponent<>
    }

    public static void RepairPortal()
    {
        GameObject portal = FindGameObjectWithTag("Portal");
        portal.GetComponent<TeleportActivator>().Activate();
    }

    private static void LookAt(GameObject gameObject)
    {
        LookAt(gameObject.GetComponent<CameraDirector>());
    }

    private static void LookAtFindable(string name)
    {
        LookAt(FindGameObjectWithName(name));
    }

    private static void LookAtTaged(string tag)
    {
        LookAt(FindGameObjectWithTag(tag));
    }

    private static void LookAt(CameraDirector director)
    {
        Transform cam = Camera.main.transform;
        Vector3 euler = cam.localEulerAngles;
        Vector3 pos = cam.localPosition;
        director.AnimateCamera(GameManager.PlayerMainCamera.transform, true, ()=>
        {
            cam.localEulerAngles = euler;
            cam.localPosition = pos;
        });
    }

    public static void LookAtMainIsland()
    {
        LookAtFindable(MAP_ONE_MAINISLAND_NAME);
    }

    public static void GetDrunk()
    {

    }

    #region Steersman events

    public static void SteersmanToLandingStage()
    {
        MoveTowards move = FindGameObjectWithTag("Steersman").GetComponent<MoveTowards>();
    }


    public static void FightSteersman()
    {
        PirateSwordFight steersmanFight = GameObject.FindGameObjectWithTag("Steersman").GetComponent<PirateSwordFight>();
        DialogTrigger d = steersmanFight.GetComponent<DialogTrigger>();
        GameManager.Story.IsFightingSteersman = true;
        PartyAffiliation player = GameManager.GetPlayerComponent<PartyAffiliation>();
        d.DisableInteraction();
        player.party = PartyAffiliation.PartyName.French;
        steersmanFight.DoDelayed(5f, delegate
        {
            GameManager.GetPlayerComponent<InterfaceController>().Clear();
            steersmanFight.AttackTarget(player);
        });

    }


    public static void Stay()
    {
        MoveTowards move = FindGameObjectWithTag("Steersman").GetComponent<MoveTowards>();
        move.target = null;
        move.enabled = false;
        move.GetComponent<Rigidbody>().isKinematic = true;

    }

    public static void Follow()
    {
        MoveTowards move = GameObject.FindGameObjectWithTag("Steersman").GetComponent<MoveTowards>();
        move.target = GameManager.GetPlayerComponent<STransform>();
        move.enabled = true;
        move.GetComponent<Rigidbody>().isKinematic = false;
    }

    #endregion

    public static void LookAtPortal()
    {
        LookAtTaged(MAP_ONE_SIMPLE_PORTAL_TAG_NAME);
    }

    public static void OpenPermuda()
    {
        GameObject f = FindGameObjectWithTag(MAP_ONE_PERMUDA_PORTAL_TAG_NAME);
        f.GetComponentInChildren<StartFog>().EnableFog();
    }

    public static void LookAtTriangle()
    {
        LookAtTaged(MAP_ONE_PERMUDA_PORTAL_TAG_NAME);
    }

    public static void MakeCrewDrunk()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Guard"))
        {
            g.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public static void ActivateSchnuckidipuz()
    {
        FindGameObjectWithTag("Schnucki").GetComponent<HideOnPlay>().Show();
    }

    public static void FightBubu()
    {
        BubuSwordFight bubuFight = FindGameObjectWithTag("Bubu").GetComponent<BubuSwordFight>();
        GameManager.Story.isFightingBubu = true;
        DialogTrigger d = bubuFight.GetComponent<DialogTrigger>();
        d.AbortInteraction();
        d.isInteractionEnabled = false;
        bubuFight.enabled = true;
        PartyAffiliation player = GameManager.GetPlayerComponent<PartyAffiliation>();
        player.party = PartyAffiliation.PartyName.French;
        bubuFight.DoDelayed(5f, delegate
        {
            GameManager.GetPlayerComponent<InterfaceController>().Clear();
            bubuFight.AttackTarget(player);
        });
    }

    public static void GiveDocument()
    {
        ItemContainer item = GameManager.GetPlayerComponent<DelayedItem>().itemContainer;
        UiInventory inventory = GameManager.GetPlayerComponent<UiInventory>();
        inventory.AddItem(item);
        AnimateItem.AnimateItems(new List<ItemContainer>() { item }, 0.2f, inventory);
    }

    public static void EndGame()
    {
        GameObject player = GameManager.Player;
        InterfaceController interfaceController = player.GetComponent<InterfaceController>();
        GameObject admiral = FindGameObjectWithTag("Admiral");
        admiral.GetComponent<DialogTrigger>().DisableInteraction();
        player.transform.parent.GetComponent<EndGameMusic>().PlayMusic();
        Transform cam = Camera.main.transform;
        interfaceController.Clear();
        GameManager.FreezePlayer();
        MonoBehaviour m = player.GetComponent<MonoBehaviour>();
        m.DoDelayed(0.25f, delegate
        {
            Vector3 observationPosition = new Vector3(334.8f, 65.49f, -205f);
            Vector3 observationEuler = new Vector3(90.00001f, 0, 0f);

            m.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 4f, v => cam.eulerAngles = v));
            m.StartCoroutine(
                SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 4f, v => cam.position = v, () =>
                {
                    interfaceController.DoDelayed(5f, delegate
                    {
                        admiral.GetComponent<IntroAdmiral>().JumpInWater();
                    });
                    Vector3 observationPosition2 = new Vector3(340.32f, 149.3f, -208.62f);
                    Vector3 observationEuler2 = new Vector3(27.514f, -50.534f, 0);
                    m.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler2, 8f, v => cam.eulerAngles = v));
                    m.StartCoroutine(
                        SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition2, 17.8f, v => cam.position = v, delegate
                        {
                            SceneSwitcher.EnterScene("MenueScene", false, false);
                        }));
                })
            );
        });
    }



}
