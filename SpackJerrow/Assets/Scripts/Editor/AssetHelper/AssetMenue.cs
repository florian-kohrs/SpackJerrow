using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssetMenue : MonoBehaviour
{

    private const string BASE_FOLDER_NAME = "ScriptableObjects/";

    [MenuItem("Assets/Create/Custom/MovementSpell")]
    public static void NewMovement()
    {
        AssetCreator.CreateAsset<MovementSpell>(BASE_FOLDER_NAME + "Movement");
    }

    [MenuItem("Assets/Create/Custom/Item/EquipableItem")]
    public static void NewEquipableItem()
    {
        AssetCreator.CreateAsset<EquipableItem>(BASE_FOLDER_NAME + "Items/Equipable");
    }

    [MenuItem("Assets/Create/Custom/Item/InventoryItem")]
    public static void NewInventoryItem()
    {
        AssetCreator.CreateAsset<InventoryItem>(BASE_FOLDER_NAME + "Items/Default");
    }
    
    [MenuItem("Assets/Create/Custom/Animation/ItemAnimation")]
    public static void NewItemAnimation()
    {
        AssetCreator.CreateAsset<MoveAnimation>(BASE_FOLDER_NAME + "Animations/Movement");
    }

    [MenuItem("Assets/Create/Custom/Animation/AttackAnimation")]
    public static void NewAttackAnimation()
    {
        AssetCreator.CreateAsset<AttackAnimation>(BASE_FOLDER_NAME + "Animations/Attack");
    }
    
    [MenuItem("Assets/Create/Custom/Item/Useable")]
    public static void NewUseableItem()
    {
        AssetCreator.CreateAsset<UseableItem>(BASE_FOLDER_NAME + "Items/Useable");
    }
    
}
