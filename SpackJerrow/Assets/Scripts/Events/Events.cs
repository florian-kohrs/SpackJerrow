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

    #region findables names

    #region mainmap

    private const string MAP_ONE_MAINISLAND_LANDINGSTAGE = "WaitPositionMainLandingStage";


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

    public static GameObject FindGameObjectWithTag(string tag)
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

        portal.GetComponent<MeshRenderer>().enabled = true;
        portal.GetComponent<BoxCollider>().enabled = true;
    }

    public static void LookAtMainIsland()
    {
        GameObject portal = FindGameObjectWithTag("Portal");

        Vector3 observationPosition = portal.transform.position + portal.transform.right * 10 + portal.transform.up * 15;
        Vector3 observationEuler = new Vector3(30.4f, -18.32f, 0);

        Transform cam = Camera.main.transform;
        Vector3 euler = cam.localEulerAngles;
        Vector3 pos = cam.localPosition;
        MonoBehaviour source = GameManager.GetPlayerComponent<MonoBehaviour>();
        source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 2.5f, v => cam.eulerAngles = v));
        source.StartCoroutine(
        SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 2.5f, v => cam.position = v, () =>
          {
              source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(observationPosition, observationPosition, 1.5f, v => { }, () =>
              {
                  cam.localEulerAngles = euler;
                  cam.localPosition = pos;
              }));
          })
          );

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
        Vector3 observationPosition = new Vector3(-253.2f, 35.7f, 119.1f);
        Vector3 observationEuler = new Vector3(22.306f, -90f, 0);

        Transform cam = Camera.main.transform;
        Vector3 pos = cam.localPosition;
        Vector3 euler = cam.localEulerAngles;
        MonoBehaviour source = GameManager.GetPlayerComponent<MonoBehaviour>();
        source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 2.5f, v => cam.eulerAngles = v));
        source.StartCoroutine(
            SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 2.5f, v => cam.position = v, () =>
            {
                source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(observationPosition, observationPosition, 1.5f, v => { }, () =>
                {
                    cam.localEulerAngles = euler;
                    cam.localPosition = pos;
                }));
            })
        );
    }
    
    public static void OpenPermuda()
    {
        GameObject f = FindGameObjectWithTag("PermudaTri");
        f.GetComponent<StartFog>().EnableFog();
    }

    public static void LookAtTriangle()
    {
        GameObject f = FindGameObjectWithTag("PermudaTri");

        Vector3 observationPosition = f.transform.position + f.transform.right * -20 + f.transform.forward * 40 + f.transform.up * 25;
        Vector3 observationEuler = new Vector3(27.237f, -149.538f, 0);

        Transform cam = Camera.main.transform;
        Vector3 euler = cam.localEulerAngles;
        Vector3 pos = cam.localPosition;
        MonoBehaviour source = GameManager.GetPlayerComponent<MonoBehaviour>();
        source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 2.5f, v => cam.eulerAngles = v));
        source.StartCoroutine(
            SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 2.5f, v => cam.position = v, () =>
            {
                source.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(observationPosition, observationPosition, 2f, v => { }, () =>
                {
                    cam.localEulerAngles = euler;
                    cam.localPosition = pos;
                }));
            })
        );
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
            Vector3 observationPosition = new Vector3(23.68f, 27.49f, -27.52f);
            Vector3 observationEuler = new Vector3(90.00001f, 0, 38.817f);

            m.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler, 4f, v => cam.eulerAngles = v));
            m.StartCoroutine(
                SmoothTransformation<Vector3>.SmoothRotateEuler(cam.position, observationPosition, 4f, v => cam.position = v, () =>
                {
                    interfaceController.DoDelayed(5f, delegate
                    {
                        admiral.GetComponent<IntroAdmiral>().JumpInWater();
                    });
                    Vector3 observationPosition2 = new Vector3(95, 255.7f, -116.1f);
                    Vector3 observationEuler2 = new Vector3(51.937f, -38.817f, 0);
                    m.StartCoroutine(SmoothTransformation<Vector3>.SmoothRotateAngleEuler(cam.eulerAngles, observationEuler2, 16f, v => cam.eulerAngles = v));
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
