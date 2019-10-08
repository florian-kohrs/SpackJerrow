using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLocker : MonoBehaviour
{

    private void Start()
    {
        HideCursor();
    }

    public static void HideCursor(CursorLockMode lockMode = CursorLockMode.Locked)
    {
        Cursor.lockState = lockMode;
        Cursor.visible = false;
    }

    public static void ShowCursorConfinded()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public static void FreeCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
