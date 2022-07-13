using UnityEngine;
using UnityEngine.UI;

public class SC_RPGFieldHud : MonoBehaviour
{
    [SerializeField] GameObject[] arrJoyPadArrow;
    [SerializeField] GameObject interactionBtn;
    [SerializeField] GameObject InformationBtn;
    [SerializeField] GameObject UserInfomationPanel;
    [SerializeField] GameObject[] arrFieldHPBar;
    GameObject FieldUnitDeadPanel;


    static SC_RPGFieldHud _uniqueInstance;

    public static SC_RPGFieldHud _instance
    {
        get { return _uniqueInstance; }
    }
    void Awake()
    {
        _uniqueInstance = this;
    }
    void Start()
    {
        HPBarUpdate();
        for (int i = 0; i < arrFieldHPBar.Length; i++)
        {
            arrFieldHPBar[i].transform.GetChild(1).GetComponent<Image>().sprite = SC_RPGInGameManager._instance.PCUnitList[i]._modelProfile;
            arrFieldHPBar[i].transform.GetChild(3).GetComponent<Text>().text = SC_RPGInGameManager._instance.PCUnitList[i]._unitName;
        }
    }

    void Update()
    {
        Interaction();
    }
    //TopUI
    public void InfoBtnTouch()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        SC_UIFunctionPool._instance.WindowOpen(UserInfomationPanel, transform.gameObject);
    }

    //MiddleUI
    public void HPBarUpdate()
    {
        for (int i = 0; i < arrFieldHPBar.Length; i++)
        {
            if (SC_RPGInGameManager._instance.PCUnitList[i]._nowHP <= 0)
                SC_RPGInGameManager._instance.PCUnitList[i]._nowHP = 0;
            arrFieldHPBar[i].transform.GetChild(2).GetComponent<Slider>().value = (float)SC_RPGInGameManager._instance.PCUnitList[i]._nowHP / (float)SC_RPGInGameManager._instance.PCUnitList[i]._maxHP;
            arrFieldHPBar[i].transform.GetChild(2).GetChild(2).GetComponent<Text>().text = SC_RPGInGameManager._instance.PCUnitList[i]._nowHP + " / " + SC_RPGInGameManager._instance.PCUnitList[i]._maxHP;
        }
    }
    public void FieldDeadPanel(int TargetArrNum)
    {
        transform.GetChild(2).GetChild(2).GetChild(TargetArrNum).GetChild(1).GetChild(0).gameObject.SetActive(true);
    }

    //BotUI
    //조작방향키 UI
    public void RightArrowDown()
    {
        if (SC_CharacterControler._Instance.IsMoving == false)
        {
            SC_CharacterControler._Instance.m_Direction = Vector3.right;
            SC_CharacterControler._Instance.FirstMove(SC_CharacterControler._Instance.m_Direction, SC_CharacterControler._Instance.IsRight, "RightMove");
        }
    }
    public void LeftArrowDown()
    {
        if (SC_CharacterControler._Instance.IsMoving == false)
        {
            SC_CharacterControler._Instance.m_Direction = Vector3.left;
            SC_CharacterControler._Instance.FirstMove(SC_CharacterControler._Instance.m_Direction, SC_CharacterControler._Instance.IsLeft, "LeftMove");
        }
    }
    public void UpArrowDown()
    {
        if (SC_CharacterControler._Instance.IsMoving == false)
        {
            SC_CharacterControler._Instance.m_Direction = Vector3.up;
            SC_CharacterControler._Instance.FirstMove(SC_CharacterControler._Instance.m_Direction, SC_CharacterControler._Instance.IsUp, "UpMove");
        }
    }
    public void DownArrowDown()
    {
        if (SC_CharacterControler._Instance.IsMoving == false)
        {
            SC_CharacterControler._Instance.m_Direction = Vector3.down;
            SC_CharacterControler._Instance.FirstMove(SC_CharacterControler._Instance.m_Direction, SC_CharacterControler._Instance.IsDown, "DownMove");
        }
    }
    public void ArrowBtnUp()
    {

        SC_CharacterControler._Instance.StayInitInfo(Vector3.zero, false, null);
        SC_CharacterControler._Instance.IsCheckStay = false;
    }

    //상호작용 키
    public void Interaction()
    {
        if (SC_CharacterControler._Instance.IsfrontEnemy == true)
        {
            interactionBtn.GetComponent<Image>().color = new Color(255, 0, 0, 170);
        }
        else
        {
            interactionBtn.GetComponent<Image>().color = new Color(255, 255, 255, 107);
        }
    }
    public void InteractionBtnTouch()
    {
        if (SC_CharacterControler._Instance.IsfrontEnemy == true)
        {
            SC_CharacterControler._Instance.EnemyInteract();
            SC_CharacterControler._Instance.IsfrontEnemy = false;
            Interaction();

        }
    }

}
