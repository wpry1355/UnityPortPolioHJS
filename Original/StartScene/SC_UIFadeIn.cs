using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_UIFadeIn : MonoBehaviour
{
    Image TargetImg;
    float FadeInPanel;
    float FadeInSound;
    string SceneName;

    private void Awake()
    {
        TargetImg = gameObject.transform.GetChild(0).GetComponent<Image>();
        TargetImg.color = new Color(0,0, 0, 1);
        FadeInPanel = 1;
        FadeInSound = 0;
    }
    private void Update()
    {
        FadeInPanel = FadeInPanel - Time.deltaTime;
        FadeInSound = FadeInSound + (1 / Time.deltaTime);
        TargetImg.color = new Color(TargetImg.color.r, TargetImg.color.g, TargetImg.color.b, FadeInPanel);
        if(FadeInPanel <0)
            Destroy(gameObject);
        

    }

}
