using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class AssetCreator
{

    /// <summary>
    /// createes an asset of type t at given path
    /// </summary>
    /// <typeparam name="T">type of asset</typeparam>
    /// <param name="path">relative path to safe assets</param>
    /// <param name="selectAsset">will focus the asset in inspector</param>
    public static void CreateAsset<T>(string path, bool selectAsset = true)
    {
        string currentPath = "Assets";
        foreach (string s in path.Split('/'))
        {
            string nextPath = Path.Combine(currentPath, s);
            if (!AssetDatabase.IsValidFolder(nextPath))
            {
                AssetDatabase.CreateFolder(currentPath, s);
            }
            currentPath = nextPath;
        }

        ScriptableObject asset = ScriptableObject.CreateInstance(typeof(T));
        AssetDatabase.CreateAsset(asset, Path.Combine(currentPath, System.Guid.NewGuid().ToString()) + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        if (selectAsset)
        {
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }
}
