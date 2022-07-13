using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_TDHelpWindow : MonoBehaviour
{
    // 캡쳐 이미지 슬롯 , 설명 텍스트 슬롯.
    [SerializeField] Image CaptureImage;
    [SerializeField] Text Explanation;
    // 왼쪽버튼 오른쪽버튼
    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;
    // 스왑할 자료들
    [SerializeField] Sprite[] CaptureImageArr;
    string[] ExplanationArr = new string[4];

    int LevelVar = 0;
    float SaveTimeScale = 0;
    void Awake()
    {
        ExplanationArr[0] = "1. 적의 공세를 함께 막을 영웅을 고를 수 있습니다.\n\n2.해당 스테이지에 등장하는 적의 종류를 알 수 있습니다.";
        ExplanationArr[1] = "1. 영웅 및 타워를 드레그하여 건설합니다. 건설 시 Energy가 소모됩니다.\n\n2.영웅 및 타워를 철거합니다.철거 시 일정량의 Energy가 회복됩니다.";
        ExplanationArr[2] = "1. 해당 라운드에서 등장 대기중인 적의 수 입니다.\n\n2.다음 라운드가 시작되기 까지의 초입니다.\n\n3.적을 놓칠 시 줄어드는 Life입니다. 0이 되면 패배합니다.";
        ExplanationArr[3] = "1. 게임 진행 속도를 조절합니다. 일시정지 부터 3배속까지 가능합니다.\n\n2.현재 보고있는 도움말 팝업을 생성합니다.\n\n3.설정창을 생성합니다.볼륨 및 로비로 이동이 가능합니다.";
        CaptureImage.sprite = CaptureImageArr[0];
        Explanation.text = ExplanationArr[0];
        LeftButton.gameObject.SetActive(false);
        SaveTimeScale = Time.timeScale;
        Time.timeScale = 0;
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
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        Time.timeScale = SaveTimeScale;
        Destroy(gameObject);
    }
}
