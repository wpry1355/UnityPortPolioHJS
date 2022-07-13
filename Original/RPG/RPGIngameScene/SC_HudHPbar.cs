using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_HudHPbar : MonoBehaviour
{
    [SerializeField] int CharacterIndex;
    Sprite CharacterImage;
    float HpMax;
    float HpCurrent;

    private void Start()
    {
        CharacterImage = SC_RPGInGameManager._instance.PCUnitList[CharacterIndex]._modelProfile;
        transform.GetChild(0).GetComponent<Image>().sprite = CharacterImage;
    }
    
    private void Update()
    {
        HPBarUpdate();
    }

    public void HPBarUpdate()
    {

        transform.GetChild(1).GetComponent<Slider>().value = (float)SC_RPGInGameManager._instance.PCUnitList[CharacterIndex]._nowHP / (float)SC_RPGInGameManager._instance.PCUnitList[CharacterIndex]._maxHP;
        transform.GetChild(1).GetChild(2).GetComponent<Text>().text = SC_RPGInGameManager._instance.PCUnitList[CharacterIndex]._nowHP + " / " + SC_RPGInGameManager._instance.PCUnitList[CharacterIndex]._maxHP;
    }
}