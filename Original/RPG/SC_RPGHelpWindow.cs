using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_RPGHelpWindow : MonoBehaviour
{
    // 캡쳐 이미지 슬롯 , 설명 텍스트 슬롯.
    [SerializeField] Image CaptureImage;
    [SerializeField] Text Explanation;
    // 왼쪽버튼 오른쪽버튼
    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;
    // 스왑할 자료들
    [SerializeField] Sprite[] CaptureImageArr;
    string[] ExplanationArr;

    int LevelVar = 0;
    void Awake()
    {
        Time.timeScale = 0;
        CaptureImage.sprite = CaptureImageArr[0];

        LeftButton.gameObject.SetActive(false);
        ExplanationArr = new string[CaptureImageArr.Length];
        ExplanationArr[0] = "1.현재 라운드입니다.\n\n2.현재 획득한 보상입니다.\n\n3.튜토리얼버튼과 설정창버튼입니다.";
        ExplanationArr[1] = "1.현재 전투 상황을 보여줍니다.\n\n2.캐릭터 행동선택창입니다. 공격/방어/스킬순이며 스킬은 쿨타임이 존재합니다.\n\n3.행동선택을 종료하고 턴을 진행합니다.\n\n4.전투에서 도망칩니다.";
        Explanation.text = ExplanationArr[0];
    }
    public void LeftBtn()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        if (LevelVar == 1)
            LeftButton.gameObject.SetActive(false);
        if (LevelVar == CaptureImageArr.Length - 1)
            RightButton.gameObject.SetActive(true);
        LevelVar--;
        CaptureImage.sprite = CaptureImageArr[LevelVar];
        Explanation.text = ExplanationArr[LevelVar];
    }
    public void RightBtn()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        if (LevelVar == CaptureImageArr.Length - 2)
            RightButton.gameObject.SetActive(false);
        if (LevelVar == 0)
            LeftButton.gameObject.SetActive(true);
        LevelVar++;
        CaptureImage.sprite = CaptureImageArr[LevelVar];
        Explanation.text = ExplanationArr[LevelVar];
    }
    public void ExitBtn()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        Time.timeScale = 2.5f;
        Destroy(gameObject);
    }
}
