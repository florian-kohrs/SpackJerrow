using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class DialogNode : ScriptableObject
{

    public string teaserText;

    public string playerText;
    public AudioClip playerClip;

    public string npcText;
    public AudioClip npcClip;

    public DialogNode leftChoice;

    public DialogNode rightChoice;

    public int dialogProgressIncrement;
    
    [Tooltip("Theese Items are removed when selecting the dialog")]
    public List<ItemContainer> requieredItemsRemove;

    [Tooltip("Theese Items are kept when selecting the dialog")]
    public List<ItemContainer> requieredItemsKeep;

    [Tooltip("Theese Items are added to the players invetory when the dialog is slected")]
    public List<ItemContainer> addedItems;
    
    /// <summary>
    /// theese functions are called from the Event class when the dialog node is selected
    /// </summary>
    public string[] triggerMemberNames;
    
    public bool LastNode
    {
        get
        {
            return !HasLeft && !HasRight;
        }
    }

    public bool IsConnectorNode
    {
        get
        {
            return HasLeft ^ HasRight;
        }
    }

    public bool IsLastNode
    {
        get
        {
            return leftChoice == null && rightChoice == null;
        }
    }

    public bool HasLeft
    {
        get
        {
            return leftChoice != null;
        }
    }

    public bool HasPlayerText
    {
        get
        {
            return !string.IsNullOrEmpty(playerText);
        }
    }

    public bool HasNpcText
    {
        get
        {
            return !string.IsNullOrEmpty(npcText);
        }
    }

    public DialogNode ConnectNode
    {
        get
        {
            if (IsConnectorNode)
            {
                if (HasLeft)
                {
                    return leftChoice;
                }
                else
                {
                    return rightChoice;
                }
            }
            else
            {
                return null;
            }
        }
    }

    public bool HasRight
    {
        get
        {
            return rightChoice != null;
        }
    }

}

