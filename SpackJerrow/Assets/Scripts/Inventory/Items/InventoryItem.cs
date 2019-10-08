using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : SaveableScriptableObject
{

    public AudioClip itemClip;
    public GameObject gameObject;
    public Sprite icon;
    public string itemName;
    public string itemDescription;
    public int value;
    public bool canBeSoled;

    public ItemType itemType;

    public enum ItemType { Food, Rare, Junk}
    
}
