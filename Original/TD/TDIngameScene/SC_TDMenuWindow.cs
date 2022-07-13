using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TDMenuWindow : MonoBehaviour
{
    [SerializeField] GameObject InformationPanel;
    [SerializeField] GameObject ConfigPanel;
    [SerializeField] GameObject ExitPanel;
    float SaveNowTimeScale;
    private void Awake()
    {
        SaveNowTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }


    public void ResumeBtnTouch()
    {
        Time.timeScale = SaveNowTimeScale;
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }
    public void Information()
    {
        SC_UIFunctionPool._instance.WindowOpen(InformationPanel,  transform.gameObject);
    }
    public void Config()
    {
        SC_UIFunctionPool._instance.WindowOpen(ConfigPanel, transform.gameObject);
    }
    public void Exit()
    {
        SC_UIFunctionPool._instance.WindowOpen(ExitPanel, transform.gameObject);
    }
}
