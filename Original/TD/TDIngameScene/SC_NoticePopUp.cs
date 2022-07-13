using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_NoticePopUp : MonoBehaviour
{
    Text Notice;
    void Awake()
    {
        Notice = transform.GetChild(0).GetComponent<Text>();
        Destroy(gameObject, 1f);
    }
    public void SetTextString(string text)
    {
        Notice.text = text;
    }
}
