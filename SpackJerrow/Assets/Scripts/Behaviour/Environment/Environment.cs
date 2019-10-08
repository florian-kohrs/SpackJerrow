using System;
using System.Linq;
using UnityEngine;

public class Environment
{
    #region singleton

    private Environment()
    {
        if (!sceneEventAdded)
        {
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += (s) =>
            {
                instance = null;
            };
            sceneEventAdded = true;
        }
        FindReferencesInScene();
    }

    private static bool sceneEventAdded = false;
    
    private static Environment instance;

    public static Environment GetInstance()
    {
        if(instance == null)
        {
            instance = new Environment();
        }
        return instance;
    }

    #endregion

    #region static access

    public static bool HasSea()
    {
        return instance != null && instance.sea != null;
    }

    public static SeaSimulator GetSea()
    {
        return GetInstance().sea;
    }

    public static Weather GetWeather()
    {
        return GetInstance().weather;
    }

    #endregion

    private const string ENVIRENTMENT_TAGNAME = "Environment";

    public Weather weather;

    public SeaSimulator sea; 

    private void FindReferencesInScene()
    {
        GameObject[] envirenmentParts = GameObject.FindGameObjectsWithTag(ENVIRENTMENT_TAGNAME);
        FindReferenceInScene(envirenmentParts, out weather);
        FindReferenceInScene(envirenmentParts, out sea);
    }

    private void FindReferenceInScene<T>(GameObject[] gs, out T result) where T : Component
    {
        result = gs
            .Select(g => g.GetComponent(typeof(T)))
            .Where(m => m != null)
            .First() as T;
    }

}
