using UnityEngine;
using UnityEngine.UI;

public class SC_HeroTabUI : MonoBehaviour
{

    int NowCharaterIndex = 0;
    static SC_HeroTabUI _uniqueinstance;

    static public SC_HeroTabUI _Instance { get { return _uniqueinstance; } }
    public int _NowCharacterIndex { get { return NowCharaterIndex; } set { NowCharaterIndex = value; } }
    //LeftUI    
    [SerializeField] public GameObject[] CharList = new GameObject[4];
    [SerializeField] public GameObject[] CharListPanel = new GameObject[4];
    [SerializeField] public GameObject[] CharSlot = new GameObject[3];
    [SerializeField] public GameObject[] CharSlotRing = new GameObject[3];
    public GameObject[] ArrCharacterObject = new GameObject[3];
    public int DragIndex;
    public Color DefaultRingColor = new Color(255, 255, 255, 157);

    //RightUI
    // 0 : InfomationTabBtn
    // 1 : LevelUpTabBtn
    // 2 : InventoryTabBtn
    [SerializeField] public GameObject[] DetailTab = new GameObject[3];


    private void Awake()
    {
        _uniqueinstance = this;
        initHeroTab();
    }


    private void initHeroTab()
    {
        for (int i = 0; i < CharList.Length; i++)
        {
            CharList[i].GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)i);
        }
        for (int i = 0; i < CharSlot.Length; i++)
        {
            ArrCharacterObject[i] = Instantiate(SC_CharacterPool._instance.characterPool[SC_UserInfoManager._instance._PCPosUnit[i]], CharSlot[i].transform);
        }
        NowCharaterIndex = SC_UserInfoManager._instance._PCPosUnit[0];
        CharSlotRing[0].GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void Update()
    {
        DetailTab[1].GetComponent<SC_RPGUnitLevelUp>().UpdateData(NowCharaterIndex);
        DetailTab[0].GetComponent<SC_CharacterInfo>().initInLobby(NowCharaterIndex);
        SelectedCharacterPanelSetting();
    }

    // LeftTab
    public void ClickProfile(int CharIndex)
    {
        SC_SoundControlManager._instance.BtnClickSound();
        int[] NowSellectedChar = NowSelectCharactersIndex();
        int CheckSellectedIndex = -1;
        NowCharaterIndex = CharIndex;
        DetailTab[0].GetComponent<SC_CharacterInfo>().initInLobby(NowCharaterIndex);
        for (int i = 0; i < NowSellectedChar.Length; i++)
        {
            if (CharIndex == NowSellectedChar[i])
            {
                CheckSellectedIndex = i;
            }
        }
        RingColorSetting(CheckSellectedIndex);

    }
    public void ClickCharacterSymbol(int Index)
    {
        SC_SoundControlManager._instance.BtnClickSound();
        NowCharaterIndex = SC_UserInfoManager._instance._PCPosUnit[Index];
        RingColorSetting(Index);
    }

    public int[] NowSelectCharactersIndex()
    {
        int[] arrData = new int[3];
        for (int i = 0; i < arrData.Length; i++)
        {
            arrData[i] = SC_UserInfoManager._instance._PCPosUnit[i];
        }
        return arrData;
    }
    public void SelectedCharacterPanelSetting()
    {
        for (int i = 0; i < CharListPanel.Length; i++)
        {
            CharListPanel[i].SetActive(false);
        }

        for (int i = 0; i < CharListPanel.Length; i++)
        {
            for (int j = 0; j < SC_UserInfoManager._instance._PCPosUnit.Length; j++)
            {
                if (i == SC_UserInfoManager._instance._PCPosUnit[j])
                {
                    CharListPanel[i].SetActive(true);
                }
            }
        }

    }
    public void RingColorSetting(int Index)
    {
        switch (Index)
        {
            case -1:
                {
                    CharSlotRing[0].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    CharSlotRing[1].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    CharSlotRing[2].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    break;
                }
            case 0:
                {
                    CharSlotRing[0].GetComponent<SpriteRenderer>().color = Color.green;
                    CharSlotRing[1].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    CharSlotRing[2].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    break;
                }
            case 1:
                {
                    CharSlotRing[0].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    CharSlotRing[1].GetComponent<SpriteRenderer>().color = Color.green;
                    CharSlotRing[2].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    break;
                }
            case 2:
                {
                    CharSlotRing[0].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    CharSlotRing[1].GetComponent<SpriteRenderer>().color = DefaultRingColor;
                    CharSlotRing[2].GetComponent<SpriteRenderer>().color = Color.green; ;
                    break;
                }
        }

    }

    //Right Tab
    public void ClickInfomation()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        DetailTab[0].SetActive(true);
        DetailTab[1].SetActive(false);
        DetailTab[2].SetActive(false);
    }
    public void ClickLevelUP()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        DetailTab[0].SetActive(false);
        DetailTab[1].SetActive(true);
        DetailTab[2].SetActive(false);
    }
    public void ClickInventory()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        DetailTab[0].SetActive(false);
        DetailTab[1].SetActive(false);
        DetailTab[2].SetActive(true);
    }


    
    public void CloseWindow()
    {
        SC_TopUI._instance.UpdateInfo();
        SC_MiddleUI._instance.UpdateData();
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        SC_UIFunctionPool._instance.WindowClose(gameObject);
    }

}
