using System.Collections.Generic;
using UnityEngine;

public class SC_UserInfoManager : MonoBehaviour
{


    // 유저 인포.
    [SerializeField] Sprite ProfileImg;
    Sprite _profileImage;
    string _userName;
    int _gold;
    int _cash;
    int _playtoken;
    const int _maxPlayToken = 10;
    float _playTokenUpdateTime = 0; 
    int SelectedStageNumber;
    public int nData;
    bool IsPlayingRPG = false;
    bool IsPlayingTD = false;
    bool IsStart = false;
    public int _SelectedStageNumber { get { return SelectedStageNumber; } set { SelectedStageNumber = value; } }
    public bool _isPlayingRPG { get { return IsPlayingRPG; } set { IsPlayingRPG = value; } }
    public bool _isPlayingTD { get { return IsPlayingTD; } set { IsPlayingTD = value; } }
    public bool _isStart { get { return IsStart; } set { IsStart = value; } }
    public int MaxPlayToken { get { return _maxPlayToken; }}

    //데이터 테이블
    public List<Dictionary<string, object>> _userinfodata;
    public List<Dictionary<string, object>> _unitidefaultInfo;
    public List<Dictionary<string, object>> _userunitlevel;
    public List<Dictionary<string, object>> _unitlevelvar;
    public List<Dictionary<string, object>> _userInventory;
    public List<Dictionary<string, object>> _ItemTable;
    public List<Dictionary<string, object>> _SkillDetail;
    public List<Dictionary<string, object>> _TDUnitStat;
    public List<Dictionary<string, object>> _TDUnitLevelVar;
    public List<Dictionary<string, object>> _TDEnemyStat;
    public List<Dictionary<string, object>> _TDEnemyLevelVar;
    public List<Dictionary<string, object>> _TDHeroSkillTable;
    public List<Dictionary<string, object>> _TDMonsterDetail;


    // 유닛 배치 및 유닛정보.

    public int[] _PCPosUnit = new int[3];
    public Sprite ProfileImage
    {
        get { return _profileImage; }
        set { _profileImage = value; }
    }
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }
    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }
    public int Cash
    {
        get { return _cash; }
        set { _cash = value; }
    }
    public int Playtoken
    {
        get { return _playtoken; }
        set { _playtoken = value; }
    }

    static SC_UserInfoManager _uniqueInstance;
    public static SC_UserInfoManager _instance
    {
        get { return _uniqueInstance; }
    }
    void InitUserInfo(List<Dictionary<string, object>> userInfo)
    {
        if ((string)userInfo[0]["ProfileImage"] == "null")
        {
            _profileImage = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)userInfo[0]["FrontPosUnit"]);
        }
        else
        {
            _profileImage = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)userInfo[0]["ProfileImage"]);
        }
        _userName = userInfo[0]["UserName"].ToString();
        _gold = (int)userInfo[0]["Gold"];
        _cash = (int)userInfo[0]["Cash"];
        _playtoken = (int)userInfo[0]["PlayToken"];
    }
    public void initUnitSet(List<Dictionary<string, object>> userInfo)
    {
        _PCPosUnit[0] = (int)userInfo[0]["FrontPosUnit"];
        _PCPosUnit[1] = (int)userInfo[0]["BackPosUnit1"];
        _PCPosUnit[2] = (int)userInfo[0]["BackPosUnit2"];
    }
    void Awake()
    {
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
        //데이터 테이블 로드
        _userinfodata = SC_CSVReader.Read("UserInfomation");
        _unitidefaultInfo = SC_CSVReader.Read("UnitInitStat");
        _unitlevelvar = SC_CSVReader.Read("UnitStatVar");
        _userunitlevel = SC_CSVReader.Read("UnitLevelData");
        _userInventory = SC_CSVReader.Read("UserInventory");
        _ItemTable = SC_CSVReader.Read("TableItem");
        _SkillDetail = SC_CSVReader.Read("SkillDetail");
        _TDUnitStat = SC_CSVReader.Read("TDUnitStat");
        _TDUnitLevelVar = SC_CSVReader.Read("TDUnitLevelVar");
        _TDEnemyStat = SC_CSVReader.Read("TDEnemyStat");
        _TDEnemyLevelVar = SC_CSVReader.Read("TDEnemyLevelVar");
        _TDHeroSkillTable = SC_CSVReader.Read("TDHeroSkillTable");
        _TDMonsterDetail = SC_CSVReader.Read("TDMonsterDetail");
        initUnitSet(_userinfodata);
        InitUserInfo(_userinfodata);
    }

    void Update()
    {
        //플레이토큰 업데이트
        if(_playtoken != _maxPlayToken)
            _playTokenUpdateTime += Time.deltaTime;

        if (_playTokenUpdateTime >= 5)
        {
            _playtoken++;
            _playTokenUpdateTime = 0;
            _userinfodata[0]["PlayToken"] = _playtoken;
            if(!_isPlayingRPG && !_isPlayingTD && IsStart)
                SC_TopUI._instance.PlayTokenUpdate();
        }        
    }

    //Inventory Func
    public void AddToInventory(List<Dictionary<string, object>> SaveTargetInven, string ItemName, int Count)
    {
        // 빈곳 찾아 넣기전에 중복체크하기.

        int DuplicateIndex = 0;
        bool IsDuplicate = InventoryDuplicate(ItemName, ref DuplicateIndex);
        if (IsDuplicate == true)
        {
            if ((int)SaveTargetInven[DuplicateIndex]["Count"] + Count > 0)
                SaveTargetInven[DuplicateIndex]["Count"] = (int)SaveTargetInven[DuplicateIndex]["Count"] + Count;
            else
            {
                SaveTargetInven[DuplicateIndex]["Count"] = "null";
                SaveTargetInven[DuplicateIndex]["Name"] = "null";
            }
        }
        else
        {
            int EmptyInventoryIndex = 0;
            int ItemIndex = 0;
            EmptyInventoryIndex = FindEmptyIndexList(_userInventory);
            ItemIndex = FindItemIndex(_ItemTable, ItemName);

            SaveTargetInven[EmptyInventoryIndex]["Name"] = _ItemTable[ItemIndex]["Name"];
            if (SaveTargetInven[EmptyInventoryIndex]["Count"].ToString() == "null")
                SaveTargetInven[EmptyInventoryIndex]["Count"] = Count;
            else
                SaveTargetInven[EmptyInventoryIndex]["Count"] = (int)SaveTargetInven[EmptyInventoryIndex]["Count"] + Count;
        }
    }

    bool InventoryDuplicate(string ItemName, ref int TargetIndex)
    {
        int DuplicateIndex = 0;
        while (DuplicateIndex <= _userInventory.Count - 1 && _userInventory[DuplicateIndex]["Name"].ToString() != ItemName)
        {
            DuplicateIndex++;
        }
        if (DuplicateIndex > _userInventory.Count - 1)
            return false;
        else
        {
            TargetIndex = DuplicateIndex;
            return true;
        }
    }
    public int FindEmptyIndexList(List<Dictionary<string, object>> Inventory)
    {
        int EmptyInventoryIndex = 0;
        while (EmptyInventoryIndex < _userInventory.Count - 1 && _userInventory[EmptyInventoryIndex]["Name"].ToString() != "null")
        {
            EmptyInventoryIndex++;
        }
        return EmptyInventoryIndex;
    }
    public int FindItemIndex(List<Dictionary<string, object>> ItemTable, string ItemName)
    {
        //아이템 찾기
        int FindItemIndex = 0;
        while (FindItemIndex < ItemTable.Count && ItemTable[FindItemIndex]["Name"].ToString() != ItemName)
        {
            FindItemIndex++;
        }
        if (FindItemIndex >= ItemTable.Count)
            return -1;
        else
            return FindItemIndex;
    }

    public void AddGold(int Count)
    {
        _gold += Count;
        _userinfodata[0]["Gold"] = _gold;
    }

    public void inventoryItem()
    {
        for (int i = 0; i < _userInventory.Count - 1; i++)
        {
            Debug.Log("i번째 아이템 " + _userInventory[i]["Name"] + _userInventory[i]["Count"]);
        }
    }

    //DummyInventory Func 
    public void InventoryUpdate(List<Dictionary<string, object>> _Dummy)
    {
        List<int> FindIndex = new List<int>();
        AddItemIndex(_Dummy, FindIndex);
        for (int i = 0; i < FindIndex.Count; i++)
            AddToInventory(_userInventory, _Dummy[FindIndex[i]]["Name"].ToString(), (int)_Dummy[FindIndex[i]]["Count"]);
        SaveData(_userInventory, "UserInventory");
        SaveData(_userinfodata, "UserInfomation");
    }

    public void AddItemIndex(List<Dictionary<string, object>> List, List<int> FindIndex)
    {
        for (int i = 0; i < List.Count; i++)
        {
            if ((int)List[i]["Count"] != 0)
                FindIndex.Add(i);
        }
    }

    public void InitDummyInventory(List<Dictionary<string, object>> List)
    {
        for (int i = 0; i < List.Count; i++)
            List[i]["Count"] = 0;
    }


    //SaveFunc
    public void SaveData(List<Dictionary<string, object>> DataList, string keyName, int Index, object ChangeValue, string SaveFileName)
    {
        DataList[Index][keyName] = ChangeValue;
        SC_CSVReader.WriteCsv(DataList, Application.persistentDataPath + "/SaveData/" + SaveFileName + ".csv", DataList.Count + 1, SC_CSVReader.getKeyCount(DataList));

    }

    public void SaveData(List<Dictionary<string, object>> DataList, string SaveFileName)
    {

        SC_CSVReader.WriteCsv(DataList, Application.persistentDataPath + "/SaveData/" + SaveFileName + ".csv", DataList.Count + 1, SC_CSVReader.getKeyCount(DataList));
    }

}
