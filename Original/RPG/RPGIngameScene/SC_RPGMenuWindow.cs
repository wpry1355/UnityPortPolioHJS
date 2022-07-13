using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RPGMenuWindow : MonoBehaviour
{
    [SerializeField] GameObject InformationPanel;
    [SerializeField] GameObject ConfigPanel;
    [SerializeField] GameObject ExitPanel;
    private void Awake()
    {
        Time.timeScale = 0;
    }

    public void ResumeBtnTouch()
    {
        Time.timeScale = 2.5f;
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }
    public void Information()
    {
        SC_UIFunctionPool._instance.WindowOpen(InformationPanel, transform.gameObject);
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
