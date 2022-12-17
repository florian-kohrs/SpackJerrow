using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : InterfaceMask
{
    
    private List<IInterfaceMask> activeMasks = 
        new List<IInterfaceMask>();
    
    public static void AddMask(IInterfaceMask mask, bool closeAllOtherMasks = false)
    {
        Interface.AddMask_(mask, closeAllOtherMasks);
    }

    public void AddMask_(IInterfaceMask mask, bool closeAllOtherMasks = false)
    {
        if (closeAllOtherMasks)
        {
            Clear();
        }
        activeMasks.Add(mask);
        ApplyMask(mask);
        mask.Open();
    }

    protected override void Start()
    {
        base.Start();
        DismissText();
    }

    public void Clear()
    {
        foreach(IInterfaceMask m in activeMasks)
        {
            m.Close();
        }
        activeMasks.Clear();
    }

    public void ForceMask(IInterfaceMask mask)
    {
        activeMasks.Add(mask);
        ApplyMask(mask);
        mask.Open();
    }

    private void ApplyMask(IInterfaceMask mask)
    {
        Cursor.lockState = mask.CursorMode;
        switch (mask.CursorMode)
        {
            case CursorLockMode.Confined:
            case CursorLockMode.None:
                Cursor.visible = true;
                break;

            case CursorLockMode.Locked:
                Cursor.visible = false;
                break;
        }
    }

    public static void RemoveMask(IInterfaceMask mask)
    {
        Interface.RemoveMask_(mask);
    }

    public void RemoveMask_(IInterfaceMask mask)
    {
        if (activeMasks[activeMasks.Count - 1] == mask)
        {
            RemoveMask();
        }
        else
        {
            activeMasks.Remove(mask);
            mask.Close();
        }
    }

    public void RemoveMask()
    {
        if (activeMasks.Count > 0)
        {
            activeMasks[activeMasks.Count - 1].Close();
            activeMasks.RemoveAt(activeMasks.Count - 1);
            if (activeMasks.Count > 0)
            {
                ApplyMask(activeMasks[activeMasks.Count - 1]);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else
        {
            AddMask(this);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RemoveMask();
        }
    }

    public override CursorLockMode CursorMode => CursorLockMode.None;

    public override bool BlockOtherInterfaces => true;

    protected override void Open_()
    {
        DismissText();
        GameManager.FreezeGame();
    }

    protected override void Close_()
    {
        GameManager.UnfreezeGame();
    }
    
    private static InterfaceController interfaceController;

    private static InterfaceController Interface
    {
        get
        {
            if (interfaceController == null)
            {
                interfaceController = GameManager.GetPlayerComponent<InterfaceController>();
            }
            return interfaceController;
        }
    }

    public static void DisplayText(string text, float duration = 2)
    {
        Interface.DisplayText_(text, duration);
    }

    [SerializeField]
    private Text text;

    [SerializeField]
    private GameObject textContainer;

    private IEnumerator textDismisser;

    private void DisplayText_(string text, float duration = 2)
    {
        if(textDismisser != null)
        {
            StopCoroutine(textDismisser);
        }
        textContainer.SetActive(true);
        this.text.text = text;
        textDismisser = this.DoDelayed(duration, delegate
        {
            DismissText();
        });
    }

    private void DismissText()
    {
        textContainer.SetActive(false);
    }
    
}
