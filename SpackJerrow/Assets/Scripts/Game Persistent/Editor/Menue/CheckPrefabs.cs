using UnityEditor;
using UnityEngine;

public class CheckPrefabs : MonoBehaviour
{

    [MenuItem("SaveablePrefabs/Validate all Prefabs")]
    static void checkAllPrefabs()
    {
        AssetPathFixer.checkAllPrefabsForSaveableEnvironment();
    }

    [MenuItem("SaveablePrefabs/Validate unchecked Prefabs")]
    static void checkUncheckedPrefabs()
    {
        AssetPathFixer.checkUncheckedPrefabsForSaveableEnvironment();
    }

    [MenuItem("SaveablePrefabs/Validate all Assets")]
    static void checkAllAssets()
    {
        AssetPathFixer.RevalidateAllSaveableObjects();
    }

}
