using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : InterfaceMask
{

    [Range(0.2f,50)]
    public float displaySpeed = 12;

    private  float DisplaySpeed
    {
        get
        {
            return displaySpeed;
        }
    }

    public AudioSource playerAudio;

    private AudioSource otherAudio;

    private DialogTrigger currentDialog;

    private DialogNode currentDialogNode;

    private int currentDialogIndex;
    
    public InterfaceController interfaceController;

    public Text choiceOne;
    public Text choiceTwo;
    public Text connectorText;
    public Text displayedPlayerText;
    public Text displayedNpcText;
    public Button leftChoiceBtn;
    public Button rightChoiceBtn;
    public Button connectBtn;
    
    //private IEnumerator currentTextAnimation;

    protected IControllableCoroutine currentTextAnimation;

    public void SelectDialogAnswer(int answerIndex)
    {
        if(answerIndex == 0)
        {
            SelectDialogNode(currentDialogNode.leftChoice);
        }
        else if(answerIndex == 1)
        {
            SelectDialogNode(currentDialogNode.rightChoice);
        }
        else
        {
            SelectDialogNode(currentDialogNode.ConnectNode);
        }
    }

    public void StartDialog(DialogTrigger dialog)
    {
        currentDialog = dialog;
        currentDialog.DisableInteractionTemp();
        otherAudio = dialog.Source;
        currentDialogNode = dialog.CurrentNode;
        interfaceController.AddMask_(this);
    }

    public void EndCurrentDialog()
    {
        interfaceController.RemoveMask_(this);
        GameManager.UnfreezeCamera();
    }

    private void SelectDialogNode(DialogNode node)
    {
        currentDialogNode = node;
        if (node == null)
        {
            EndCurrentDialog();
        }
        else
        {
            ProcessNode(node);
        }
    }

    private void ProcessNode(DialogNode node)
    {
        HideDialogChoices();
        currentDialog.IncreaseDialogProgress(node.dialogProgressIncrement);
        inventory.RemoveItems(node.requieredItemsRemove);
        
        displayedPlayerText.text = string.Empty;
        displayedNpcText.text = string.Empty;
         DisplayText(currentDialogNode.playerText, displayedPlayerText, playerAudio, node.playerClip,
            ()=>
            {
                DisplayText(currentDialogNode.npcText,displayedNpcText, otherAudio, node.npcClip,
                    ()=>
                    {
                        ShowDialogChoices();
                    });
            });
        CallNodeEvent(node);
    }

    private void CallNodeEvent(DialogNode node)
    {
        if (node.triggerMemberNames != null)
        {
            foreach (string s in node.triggerMemberNames)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    typeof(Events).GetMethod(s).Invoke(null, new object[] { });
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("PrimaryAction") && currentTextAnimation!= null && currentTextAnimation.IsPlaying)
        {
            currentTextAnimation.Finish();
        }
    }

    private void DisplayText(string text,Text uiText, AudioSource source, AudioClip clip, System.Action onFinish = null)
    {
        currentTextAnimation?.Stop();
        if (!string.IsNullOrEmpty(text))
        {
            source.clip = clip;
            source.Play();
            currentTextAnimation = new ControllableCoroutine<string>("", text, text.Length / DisplaySpeed,
                (t) => uiText.text = t,
                (s, t, p) =>
                {
                    return t.Substring(0, Mathf.RoundToInt(t.Length * p));
                },
                onFinish);
            currentTextAnimation.Start(this);
        }
        else
        {
            onFinish?.Invoke();
        }
    }

    public Inventory inventory;

    private void HideDialogChoices()
    {
        leftChoiceBtn.gameObject.SetActive(false);
        rightChoiceBtn.gameObject.SetActive(false);
        connectBtn.gameObject.SetActive(false);
    }
    
    private void ShowDialogChoices()
    {
        if (currentDialogNode.IsLastNode)
        {
            ShowConnector("See ya!");
        }
        else if (currentDialogNode.IsConnectorNode)
        {
            ShowConnector(currentDialogNode.ConnectNode.teaserText);
        }
        else/* if (currentDialogNode.HasLeft && currentDialogNode.HasRight)*/
        {
            leftChoiceBtn.gameObject.SetActive(true);
            rightChoiceBtn.gameObject.SetActive(true);
            SetNodePreviewText(leftChoiceBtn, choiceOne, currentDialogNode.leftChoice);
            SetNodePreviewText(rightChoiceBtn, choiceTwo, currentDialogNode.rightChoice);
        }
    }

    private void SetNodePreviewText(Button b, Text t, DialogNode node)
    {
        string buttonString = node.teaserText;
        bool buttonEnabled = true;

        buttonEnabled = ItemChecker(ref buttonString, "[-", "]", node.requieredItemsRemove);
        buttonEnabled = ItemChecker(ref buttonString, "(", ")", node.requieredItemsKeep) && buttonEnabled;

        t.text = buttonString;
        b.enabled = buttonEnabled;
    }

    private bool ItemChecker(ref string s, string start, string end, IList<ItemContainer> cs)
    {
        bool hasItems = true;
        if (cs.Count > 0)
        {
            s += start;
            bool setComma = false;
            foreach (ItemContainer c in cs)
            {
                if (setComma)
                {
                    s += ",";
                }
                s += c.itemCount + " x " + c.item.RuntimeRef.itemName;
                hasItems = hasItems && inventory.HasItem(c);
                setComma = true;
            }
            s += end;
        }
        return hasItems;
    }
    
    private void ShowConnector(string message)
    {
        connectorText.text = message;
        connectBtn.gameObject.SetActive(true);
    }

    protected override void Open_()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        ProcessNode(currentDialogNode);
        GameManager.FreezePlayer();
        GameManager.FreezeCamera();
    }

    public override CursorLockMode CursorMode => CursorLockMode.None;
    
    protected override void Close_()
    {
        currentDialog.EnableInteractionTemp();
        currentDialog = null;
        currentDialogNode = null;
        currentTextAnimation?.Stop();
        GetComponent<Rigidbody>().isKinematic = false;
        GameManager.UnfreezePlayer();
        GameManager.UnfreezeCamera();
    }
}
