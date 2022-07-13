using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SC_TDBattleUI : MonoBehaviour
{
    public int Life;
    public int EnemyCount;
    public int RoundCount;
    public float RemainingTime;
    public float GameSpeed;
    public int Energy;
    public bool isHeroTowerBuild;

    // Top UI init
    [SerializeField] Text WaitSummonVarWindow;
    [SerializeField] Text WaitTimeWindow;
    [SerializeField] Text RoundWindow;
    [SerializeField] Text LifeWindow;
    [SerializeField] GameObject GameSpeenBtn;
    [SerializeField] GameObject PrefebSettingWindow;
    [SerializeField] GameObject PrefebHelpWindow;

    // Middle UI init
    [SerializeField] GameObject NoticePopUpObj;
    [SerializeField] GameObject FinalResultWindow;
    [SerializeField] GameObject PausePanel;


    // Bottom UI init
    [SerializeField] GameObject HeroTowerPanel;
    [SerializeField] GameObject NormalTowerPanel;
    [SerializeField] GameObject AirTowerPanel;
    [SerializeField] Image HeroTowerImage;
    [SerializeField] Image NormalTowerImage;
    [SerializeField] Image AirTowerImage;
    [SerializeField] Text EnergyText;
    [SerializeField] GameObject DelmolitionPanel;
    [SerializeField] public GameObject sRangeRing;
    public GameObject DemolitionTargetTower;


    static SC_TDBattleUI _uniqueInstance;
    public static SC_TDBattleUI _Instance
    {
        get { return _uniqueInstance; }
    }


    private void Awake()
    {
        _uniqueInstance = this;
        isHeroTowerBuild = false;
        UpdateWaitSummonVarWindow();
        UpdateRoundWindow();
        UpdateWaitTimeWindow();
        UpdateLifeWindow();

    }


    void TopDataUpDate()
    {
        Life = SC_TDBattle._instance.Life;
        EnemyCount = SC_TDBattle._instance.WaitSpawnCount;
        RoundCount = SC_TDBattle._instance.Round;
        RemainingTime = SC_TDBattle._instance.RoundTime;
        GameSpeed = Time.timeScale;
    }
    public void BotUIInit()
    {
        HeroTowerImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)SC_TDInGameManager._instance._selectHeroIndex);
        HeroTowerImage.transform.GetChild(0).GetComponent<Text>().text = "Cost " + SC_UserInfoManager._instance._TDUnitStat[SC_TDInGameManager._instance._selectHeroIndex]["Cost"];
        NormalTowerImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)110);
        NormalTowerImage.transform.GetChild(0).GetComponent<Text>().text = "Cost " + SC_UserInfoManager._instance._TDUnitStat[4]["Cost"];
        AirTowerImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)111);
        AirTowerImage.transform.GetChild(0).GetComponent<Text>().text = "Cost " + SC_UserInfoManager._instance._TDUnitStat[5]["Cost"];
        AddEnergy((int)SC_TDInGameManager._instance._TDStageInfoData[SC_TDInGameManager._instance._stageNumIndex]["Energy"]);
        HeroPanelUpdate();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TowerTouch();
        }
    }

    //Tower Touch관련 Func
    private void TowerTouch()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray2D ray = new Ray2D(mousePos, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);

        List<RaycastResult> results = new List<RaycastResult>();
        GraphicRaycaster BotUIgraphicRay = transform.GetChild(2).GetComponent<GraphicRaycaster>();
        GraphicRaycaster TopUIgraphicRay = transform.GetChild(1).GetComponent<GraphicRaycaster>();
        PointerEventData UIPED = new PointerEventData(null);
        UIPED.position = Input.mousePosition;
        bool IsUITouch = false;

        bool tmp1 = UITouchJudge(BotUIgraphicRay, UIPED, results);
        bool tmp2 = UITouchJudge(TopUIgraphicRay, UIPED, results);
        results.Clear();
        IsUITouch = tmp1 || tmp2;
        if (hit.collider.tag == "TDTower")
        {

            for (int i = 0; i < SC_TDBattle._instance.BuildedTowerList.Count; i++)
                if (SC_TDBattle._instance.BuildedTowerList[i] == hit.collider.transform.parent.gameObject)
                {
                    if (DemolitionTargetTower != null)
                    {
                        DemolitionTargetTower.GetComponent<SC_TDUnit>()._sRangeRound.SetActive(false);
                        DemolitionTargetTower.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1,1 ,1, 1);
                        DemolitionTargetTower = SC_TDBattle._instance.BuildedTowerList[i];
                        DemolitionTargetTower.GetComponent<SC_TDUnit>()._sRangeRound.SetActive(true);
                        DemolitionTargetTower.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.green;
                    }
                    else
                    {
                        DemolitionTargetTower = SC_TDBattle._instance.BuildedTowerList[i];
                        DemolitionTargetTower.GetComponent<SC_TDUnit>()._sRangeRound.SetActive(true);
                        DemolitionTargetTower.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.green;
                    }
                }
            DemolitionPanelUpdate();
        }
        else if (IsUITouch == false)
        {
            for (int i = 0; i < SC_TDBattle._instance.BuildedTowerList.Count; i++)
            {
                SC_TDBattle._instance.BuildedTowerList[i].transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                SC_TDBattle._instance.BuildedTowerList[i].GetComponent<SC_TDUnit>()._sRangeRound.SetActive(false);
            }
            DemolitionTargetTower = null;
            DemolitionPanelUpdate();
        }
    }
    private bool UITouchJudge(GraphicRaycaster UICanvas, PointerEventData UIPED, List<RaycastResult> Result)
    {
        UICanvas.Raycast(UIPED, Result);
        for (int i = 0; i < Result.Count; i++)
        {
            if (Result[i].gameObject.tag == "TDUI")
            {
                return true;
            }
        }
        return false;
    }
    // Top UI Update Func
    public void UpdateWaitSummonVarWindow()
    {
        TopDataUpDate();
        WaitSummonVarWindow.text = EnemyCount.ToString();
    }
    public void UpdateWaitTimeWindow()
    {
        TopDataUpDate();
        WaitTimeWindow.text = ((int)RemainingTime).ToString();
    }
    public void UpdateRoundWindow()
    {
        TopDataUpDate();
        RoundWindow.text = "Round  " + RoundCount.ToString();
    }
    public void UpdateLifeWindow()
    {
        TopDataUpDate();
        LifeWindow.text = Life.ToString();
    }
    public void AddEnergy(int Value)
    {
        Energy = Energy + Value;
        EnergyText.text = Energy.ToString();
        HeroPanelUpdate();
        TowerPanelUpdate();
    }

    public void SettingBtnClick()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        SC_UIFunctionPool._instance.WindowOpen(PrefebSettingWindow, transform.gameObject);
    }
    public void GameSpeedBtnClick()
    {

        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        //노말 버튼을 배속 버튼으로 바꾸기.
        if (Time.timeScale == 0)
            PausePanelOff();

        switch (Time.timeScale)
        {
            case 0:
                Time.timeScale = 1;
                break;
            case 1:
                Time.timeScale = 2;
                break;
            case 2:
                Time.timeScale = 3;
                break;
            case 3:
                Time.timeScale = 0;
                PausePanelOn();
                break;

        }
        GameSpeed = Time.timeScale;
        GameSpeenBtn.transform.GetChild(0).GetComponent<Text>().text = "X" + GameSpeed.ToString();
        if(GameSpeed == 0)
        {
            GameSpeenBtn.transform.GetChild(0).GetComponent<Text>().text = "||";
        }
    }
    public void OpenHelpWindow()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        SC_UIFunctionPool._instance.WindowOpen(PrefebHelpWindow, transform.GetChild(0).gameObject);
    }
    void PausePanelOn()
    {
        PausePanel.gameObject.SetActive(true);
        HeroTowerPanel.SetActive(true);
        AirTowerPanel.SetActive(true);
        NormalTowerPanel.SetActive(true);
    }

    void PausePanelOff()
    {
        PausePanel.gameObject.SetActive(false);
        HeroPanelUpdate();
        TowerPanelUpdate();
    }



    // Bot UI Update Func
    public void HeroPanelUpdate()
    { 
        if (isHeroTowerBuild == true)
            HeroTowerPanel.SetActive(true);
        else if((int)SC_UserInfoManager._instance._TDUnitStat[SC_TDInGameManager._instance._selectHeroIndex]["Cost"] > Energy)
            HeroTowerPanel.SetActive(true);
        else
            HeroTowerPanel.SetActive(false);
    }

    public void TowerPanelUpdate()
    {
        if((int)SC_UserInfoManager._instance._TDUnitStat[4]["Cost"]>Energy)
            NormalTowerPanel.SetActive(true);
        else
            NormalTowerPanel.SetActive(false);


        if ((int)SC_UserInfoManager._instance._TDUnitStat[5]["Cost"] > Energy)
            AirTowerPanel.SetActive(true);
        else
            AirTowerPanel.SetActive(false);
    }

    public void DemolitionBtnClick()
    {
        //철거 기점.
        if (DemolitionTargetTower != null)
        {
            SC_TDUnit TargetSC = DemolitionTargetTower.GetComponent<SC_TDUnit>();
            if ((int)(SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), TargetSC._unitName) < 110)
            {
                //삭제할 타워가 히어로 타워 일경우 기점
                isHeroTowerBuild = false;                
            }
            SC_TDBattle._instance.BuildedTowerList.Remove(DemolitionTargetTower);
            AddEnergy( TargetSC._cost / 2);
            Destroy(TargetSC._sRangeRound);
            Destroy(DemolitionTargetTower);
            DemolitionPanelUpdate();
        }
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
    }

    public void DemolitionPanelUpdate()
    {
        if(DemolitionTargetTower != null)
            DelmolitionPanel.SetActive(false);
        else
            DelmolitionPanel.SetActive(true);
    }



    //Middle UI Func
    public void NoticePopUp(string text)
    {
        GameObject go = Instantiate(NoticePopUpObj, transform.GetChild(0));
        go.GetComponent<SC_NoticePopUp>().SetTextString(text);
    }
    public void OpenResultWindow(bool IsWin)
    {
        if (IsWin)
        {
            for (int i = 0; i < SC_TDInGameManager._instance._TDDummyInventory.Count; i++)
                SC_TDInGameManager._instance._TDDummyInventory[i]["Count"] = Random.Range(1, SC_TDBattle._instance.Round+1);
        }
        else
        {
            for (int i = 0; i < SC_TDInGameManager._instance._TDDummyInventory.Count; i++)
                SC_TDInGameManager._instance._TDDummyInventory[i]["Count"] = Random.Range(0, SC_TDBattle._instance.Round);
        }

        SC_UIFunctionPool._instance.WindowOpen(FinalResultWindow,transform.GetChild(0).gameObject);
    }


}
