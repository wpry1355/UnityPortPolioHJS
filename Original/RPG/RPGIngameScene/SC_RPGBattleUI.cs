using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_RPGBattleUI : MonoBehaviour
{
    [SerializeField] Image[] CharacterImg = new Image[3];
    [SerializeField] Slider[] CharacterHpbar = new Slider[3];
    [SerializeField] Toggle[] _frontBattleModeTG = new Toggle[3];
    [SerializeField] Toggle[] _back1BattleModeTG = new Toggle[3];
    [SerializeField] Toggle[] _back2BattleModeTG = new Toggle[3];
    [SerializeField] Text _turnText;
    [SerializeField] GameObject[] _buttonPanel;
    [SerializeField] GameObject _battlePhaseNotice;
    [SerializeField] Slider[] _skillSlider;
    [SerializeField] GameObject[] DeadPanel;
    [SerializeField] GameObject ResultWnd;
    [SerializeField] GameObject TextHud;
    [SerializeField] GameObject RewordPrefeb;
    [SerializeField] Text UIRewordCount;
    [SerializeField] GameObject[] RewordItemPosition = new GameObject[4];
    [SerializeField] GameObject RewordItemIcon;
    [SerializeField] GameObject PrefebTutorial;
    [SerializeField] GameObject SettingWndObj;
    [SerializeField] GameObject MSHPBarPref;
    [SerializeField] GameObject DefensMarkPref;
    List<Slider> MSHPBar = new List<Slider>();
    List<GameObject> PCDefenseMark = new List<GameObject>();
    Text ResultTxt;
    Text[] _skillCosttxt = new Text[3];
    Text _battlePhaseNoticeTxt;
    bool isToggleReset = false;

    public int RewordCount = 0;
    public struct RewordItem
    {
        public string ItemName;
        public int Count;
    }

    public RewordItem getItemInfo;
    public Toggle[] FrontBattleModeTG { get { return _frontBattleModeTG; } }
    public Toggle[] Back1BattleModeTG { get { return _back1BattleModeTG; } }
    public Toggle[] Back2BattleModeTG { get { return _back2BattleModeTG; } }
    public Text TurnText { get { return _turnText; } }
    public GameObject[] ButtonPanel { get { return _buttonPanel; } }
    public Slider[] SkillSlider { get { return _skillSlider; } }
    public List<GameObject> _PCDefenseMark { get { return PCDefenseMark; } }
    static SC_RPGBattleUI _uniqueInstance;
    public static SC_RPGBattleUI _instance { get { return _uniqueInstance; } }


    void Awake()
    {
        _uniqueInstance = this;
        if (SC_RPGBattle._instance.IsBoss)
            _buttonPanel[1].SetActive(true);
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            CharacterImg[i].sprite = SC_RPGInGameManager._instance.PCUnitList[i]._modelProfile;
            _skillCosttxt[i] = _skillSlider[i].transform.GetChild(1).GetComponent<Text>();
            _skillCosttxt[i].text = "Cost " + SC_RPGInGameManager._instance.PCUnitList[i]._skillTurn.ToString();
            SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn = SC_RPGInGameManager._instance.PCUnitList[i]._skillTurn;
        }
        _battlePhaseNoticeTxt = _battlePhaseNotice.transform.GetChild(0).GetComponent<Text>();
        ResultTxt = ResultWnd.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
        getItemInfo.ItemName = "null";
        getItemInfo.Count = 0;
        DefenseMarkinit();
        UpdateSkillCost();
        UpdateHPbar();
    }

    //UI HPBAR Func
    public void UpdateMSHPBar(int MSindex)
    {
        if (SC_RPGInGameManager._instance.MSUnitList[MSindex]._nowHP <= 0)
            MSHPBar[MSindex].gameObject.SetActive(false);
        else
        {
            if (MSHPBar[MSindex].gameObject.activeSelf == false)
                MSHPBar[MSindex].gameObject.SetActive(true);
            MSHPBar[MSindex].value = SC_RPGInGameManager._instance.MSUnitList[MSindex]._HPRate;
        }
    }
    public void HPbarinit(float ScaleX = 1, float ScaleY = 1)
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.MSUnitList.Length; i++)
        {
            GameObject HPbar = Instantiate(MSHPBarPref, transform.GetComponent<Canvas>().transform);
            RectTransform HPBarRectPos = HPbar.GetComponent<RectTransform>();
            HPBarRectPos.localScale = new Vector3(ScaleX, ScaleY, 1);
            HPBarRectPos.position = SC_RPGInGameManager._instance.BattleCamera.WorldToScreenPoint(new Vector3(SC_RPGInGameManager._instance.MSUnitList[i].gameObject.transform.position.x, SC_RPGInGameManager._instance.MSUnitList[i].gameObject.transform.position.y - 0.5f, 0));
            MSHPBar.Add(HPbar.GetComponent<Slider>());
            HPbar.SetActive(false);
        }
    }
    public void UpdateHPbar()
    {
        for (int i = 0; i < 3; i++)
            CharacterHpbar[i].value = (float)SC_RPGInGameManager._instance.PCUnitList[i]._nowHP / (float)SC_RPGInGameManager._instance.PCUnitList[i]._maxHP;
    }

    //UI MSHPBAR Func
    public void MSHPBarOnOff(bool IsOn)
    {
        if (IsOn)
        {
            for (int i = 0; i < MSHPBar.Count; i++)
            {
                if (SC_RPGInGameManager._instance.MSUnitList[i]._HPRate != 1 && SC_RPGInGameManager._instance.MSUnitList[i]._nowHP > 0)
                    MSHPBar[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < MSHPBar.Count; i++)
            {
                MSHPBar[i].gameObject.SetActive(false);
            }
        }
    }

    //하단부 Toggle 관련

    public void UpdateSkillCost()
    {
        for (int i = 0; i < _skillCosttxt.Length; i++)
        {
            _skillCosttxt[i].text = "Cost " + SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn.ToString();
            _skillSlider[i].value = SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn;
        }
    }
    void ToggleReset()
    {
        isToggleReset = true;
        _frontBattleModeTG[0].isOn = true;
        _back1BattleModeTG[0].isOn = true;
        _back2BattleModeTG[0].isOn = true;
        isToggleReset = false;
    }
    public void SelectCompleteBtn()
    {
        //아군 턴 종료 기점
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_RPGBattle._instance.SelectBattleMode();
        BtnOff();
        ToggleReset();
    }
    public void EscapeBtn()
    {
        //아군 도망 버튼 클릭 기점
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        BtnOff();
        int rd = Random.Range(1, 11);
        switch (rd)
        {
            case 1:
            case 2:
                {
                    SC_RPGBattle._instance.EscapeFail();
                    return;
                }
            default:
                {
                    SC_RPGBattle._instance.EscapeSuccess();
                    return;
                }
        }
    }
    public IEnumerator BtnOn()
    {
        StartCoroutine(Notice("Player Turn!!", 1.5f));
        yield return new WaitForSeconds(2f);
        _buttonPanel[0].SetActive(false);
        if (!SC_RPGBattle._instance.IsBoss)
            _buttonPanel[1].SetActive(false);
        MSHPBarOnOff(true);
    }
    public void BtnOff()
    {
        for (int i = 0; i < _buttonPanel.Length; i++)
        {
            _buttonPanel[i].SetActive(true);
        }
    }


    public void DeadUnitUI(int TargetNum)
    {
        switch (TargetNum)
        {
            case 0:
                DeadPanel[TargetNum].gameObject.SetActive(true);
                _frontBattleModeTG[2].transform.GetChild(1).gameObject.SetActive(false);
                _frontBattleModeTG[0].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                break;
            case 1:
                DeadPanel[TargetNum].gameObject.SetActive(true);
                _back1BattleModeTG[2].transform.GetChild(1).gameObject.SetActive(false);
                _back1BattleModeTG[0].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                break;
            case 2:
                DeadPanel[TargetNum].gameObject.SetActive(true);
                _back2BattleModeTG[2].transform.GetChild(1).gameObject.SetActive(false);
                _back2BattleModeTG[0].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    public void DefenseMarkinit(float ScaleX = 1, float ScaleY = 1)
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            GameObject DefenseMark = Instantiate(DefensMarkPref, transform.GetComponent<Canvas>().transform);
            RectTransform DefenseMarkRectPos = DefenseMark.GetComponent<RectTransform>();
            DefenseMarkRectPos.localScale = new Vector3(ScaleX, ScaleY, 1);
            DefenseMarkRectPos.position = SC_RPGInGameManager._instance.BattleCamera.WorldToScreenPoint(new Vector3(SC_RPGInGameManager._instance.PCUnitList[i].gameObject.transform.position.x, SC_RPGInGameManager._instance.PCUnitList[i].gameObject.transform.position.y - 0.5f, 0));
            PCDefenseMark.Add(DefenseMark);
            DefenseMark.SetActive(false);
        }
    }

    public IEnumerator Notice(string text, float sec)
    {
        _battlePhaseNoticeTxt.text = text;
        _battlePhaseNotice.SetActive(true);
        yield return new WaitForSeconds(sec);
        _battlePhaseNotice.SetActive(false);
    }


    public void CreatTextHud(SC_RPGUnit target, string text, Color textColor)
    {
        float addheight;
        if (target._campType || target._unitName == "Skeleton")
            addheight = 4f;
        else
            addheight = 3f;

        TextHud.GetComponent<Text>().text = text;
        TextHud.GetComponent<Text>().color = textColor;
        GameObject Text = Instantiate(TextHud.gameObject, transform.GetComponent<Canvas>().transform);
        RectTransform TMRectPos = Text.GetComponent<RectTransform>();
        TMRectPos.localScale = new Vector3(1, 1, 1);
        TMRectPos.position = SC_RPGInGameManager._instance.BattleCamera.WorldToScreenPoint(new Vector3(target.transform.position.x, target.transform.position.y + addheight, 0));
        Destroy(Text, 1.5f);
    }


    //배틀 보상 관련
    public void CreateReword(Transform _transform)
    {
        Instantiate(RewordPrefeb, _transform.position, Quaternion.identity);
    }
    public void UpdateReword()
    {
        RewordCount++;
        UIRewordCount.text = RewordCount.ToString();
    }
    void RewordIteminit()
    {
        GameObject tmp;
        List<int> FindIndex = new List<int>();
        SC_UserInfoManager._instance.AddItemIndex(SC_RPGInGameManager._instance._BattleInventory, FindIndex);
        for (int i = 0; i < FindIndex.Count; i++)
        {
            tmp = Instantiate(RewordItemIcon, RewordItemPosition[i].transform);
            tmp.GetComponent<SC_GetItemIcon>().initItemInfo(FindIndex[i], SC_RPGInGameManager._instance._BattleInventory[FindIndex[i]]["Count"].ToString(), SC_RPGInGameManager._instance._BattleInventory);
        }
    }
    public RewordItem RewordItemSet(bool isboss)
    {
        if (isboss == false)
        {
            int RandomVar = Random.Range(1, 5);
            getItemInfo.ItemName = SC_UserInfoManager._instance._ItemTable[RandomVar]["Name"].ToString();
        }
        else
            getItemInfo.ItemName = SC_UserInfoManager._instance._ItemTable[0]["Name"].ToString();
        getItemInfo.Count = Random.Range(1, 3);
        return getItemInfo;
    }
    //보상 UI
    public void SetActiveResultWindow(string result)
    {
        for (int i = 0; i < RewordCount; i++)
        {
            SC_RPGInGameManager._instance.GetItemSaveDummy(RewordItemSet(false));
        }

        RewordIteminit();
        SC_RPGInGameManager._instance.DummySaveAndInit();
        ResultWnd.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = result;
        ResultWnd.SetActive(true);
    }
    public void ResultWindowEnterBtn()
    {
        SC_RPGInGameManager._instance.BattleEnd();
        ResultWnd.SetActive(false);
    }



    // UI 인터렉트 함수
    public void ConfigBtn()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        Instantiate(SettingWndObj, transform);
    }
    public void TutorialBtnTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        Instantiate(PrefebTutorial, transform);
    }
    public void ToggleTouch()
    {
        if (!isToggleReset)
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
    }
}
