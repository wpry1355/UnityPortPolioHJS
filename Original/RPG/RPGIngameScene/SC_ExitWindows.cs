using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_ExitWindows : MonoBehaviour
{

    public void yesBtn()
    {
        Time.timeScale = 1;
        SC_UIFunctionPool._instance.loadScene("LobbyScene");
    }
    public void noBtn()
    {
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }

}
