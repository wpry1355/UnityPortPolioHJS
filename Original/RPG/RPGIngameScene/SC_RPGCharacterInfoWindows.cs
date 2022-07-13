using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RPGCharacterInfoWindows : MonoBehaviour
{
    public void CloseWindow()
    {
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }
}
