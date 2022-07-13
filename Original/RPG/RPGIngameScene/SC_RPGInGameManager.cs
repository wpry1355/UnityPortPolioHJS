using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RPGInGameManager : MonoBehaviour
{
    Camera MainCamera;
    [SerializeField] public Camera BattleCamera;
    [SerializeField] public GameObject FieldHud;
    [SerializeField] public GameObject BattleHud;
    [SerializeField] GameObject[] _userTeamSpawnPos;
    [SerializeField] GameObject[] _enemyTeamSpawnPos;
    [SerializeField] public GameObject _skillPos;
    [SerializeField] GameObject[] PrefebStageMapTable = new GameObject[2];
    [SerializeField] GameObject PrefebFinalResultWindow;
    [SerializeField] GameObject PrefebPlayerSymbol;

    GameObject[] SymbolSpawnPosition = new GameObject[3];
    GameObject PlayerCharcter;
    GameObject PlayerCharacterPosition;
    GameObject StageMap;
    public int KillScore = 0;
    SC_RPGUnit[] _PCUnitList = new SC_RPGUnit[3];
    SC_RPGUnit[] _MSUnitList = new SC_RPGUnit[3];
    public bool IsBattle = false;
    int[] _PCPos = new int[3];
    int[] _MSPos = new int[3];


    public int MapVar;

    public List<Dictionary<string, object>> _SpawnData;
  
    private int SymbolSpawnVar;
    private int MonsterSymbolInfo;



    // 인벤토리
    public List<Dictionary<string, object>> _RPGDummyInventory;
    public List<Dictionary<string, object>> _BattleInventory;


    public SC_RPGUnit[] PCUnitList
    {
        get { return _PCUnitList; }
    }
    public SC_RPGUnit[] MSUnitList
    {
        get { return _MSUnitList; }
    }


    GameObject Window;
    static SC_RPGInGameManager _uniqueInstance;


    public static SC_RPGInGameManager _instance
    {
        get { return _uniqueInstance; }
    }
    void Awake()
    {
        Time.timeScale = 2.5f;
        _uniqueInstance = this;
        BattleCamera.enabled = false;
        MapVar = SC_UserInfoManager._instance._SelectedStageNumber;
        _SpawnData = SC_CSVReader.Read("MonsterSimbolPool");
        _RPGDummyInventory = SC_CSVReader.Read("RPGDummyInventory");
        _BattleInventory = SC_CSVReader.Read("RPGDummyInventory");


        initSpawnPosition();
        PlayerAndEnemySymbolSpawn();
        _PCPos = SC_UserInfoManager._instance._PCPosUnit;
        SpawnInfoInit(_PCPos, _userTeamSpawnPos, true);
        SC_SoundControlManager._instance.BGMSoundPlay(SC_PublicDefine.eSoundTrack.RPGFieldBGM);

    }


    //스테이지 스폰 관련

    //맵
    void initSpawnPosition()
    {
        StageMap = Instantiate(PrefebStageMapTable[MapVar], Vector3.zero, Quaternion.identity, transform.parent);
        SymbolSpawnPosition[0] = StageMap.transform.GetChild(1).gameObject;
        SymbolSpawnPosition[1] = StageMap.transform.GetChild(2).gameObject;
        SymbolSpawnPosition[2] = StageMap.transform.GetChild(3).gameObject;
        PlayerCharacterPosition = StageMap.transform.GetChild(4).gameObject;
    }
    //플레이어
    void PlayerAndEnemySymbolSpawn()
    {
        PrefebPlayerSymbol = SC_CharacterPool._instance.characterPool[(int)SC_UserInfoManager._instance._userinfodata[0]["FrontPosUnit"]+20];
        PlayerCharcter = Instantiate(PrefebPlayerSymbol, PlayerCharacterPosition.transform.position, Quaternion.identity);
        MainCamera = PlayerCharcter.transform.GetChild(0).GetComponent<Camera>();
        MonsterSymbolSpawn(MapVar, SymbolSpawnPosition[0]);
        MonsterSymbolSpawn(MapVar, SymbolSpawnPosition[1]);
        FieldBossSymbolSpawn(MapVar, SymbolSpawnPosition[2]);
    }

    //몬스터
    public void MonsterSymbolSpawn(int StageVar, GameObject SpawnPosition)
    {
        int SimbolVar = 20;
        SymbolSpawnVar = Random.Range(0 + StageVar * 3, 3 + StageVar * 3);
        MonsterSymbolInfo = (int)_SpawnData[SymbolSpawnVar]["전위"];
        Instantiate(SC_CharacterPool._instance.characterPool[MonsterSymbolInfo + SimbolVar], SpawnPosition.transform.position, Quaternion.identity);
    }

    public void FieldBossSymbolSpawn(int MapVar, GameObject SpawnPosition)
    {
        int SimbolVar = 20;

        SymbolSpawnVar = MapVar + 10;
        MonsterSymbolInfo = (int)_SpawnData[SymbolSpawnVar]["전위"];
        Instantiate(SC_CharacterPool._instance.characterPool[MonsterSymbolInfo + SimbolVar], SpawnPosition.transform.position, Quaternion.identity);
    }

    //배틀 시작 함수
    public void BattleIn(int[] MonsterInfo)
    {
        IsBattle = true;
        MainCamera.enabled = false;
        BattleCamera.enabled = true;
        FieldHud.SetActive(false);

        SC_UIFunctionPool._instance.WindowOpen(BattleHud, transform.gameObject);
        _MSPos[0] = MonsterInfo[0];
        _MSPos[1] = MonsterInfo[1];
        _MSPos[2] = MonsterInfo[2];
        _PCPos = SC_UserInfoManager._instance._PCPosUnit;
        if (_MSPos[0] >= 14)
            SC_RPGBattle._instance.IsBoss = true;
            
        // BG 이미지 변경
        StartCoroutine(EnemyUnitSpawn());
    }

    // 배틀 종료
    public void BattleEnd()
    {
        MainCamera.enabled = true;
        BattleCamera.enabled = false;
        FieldHud.SetActive(true);
        IsBattle = false;
        SC_RPGFieldHud._instance.HPBarUpdate();
        for (int i = 0; i < MSUnitList.Length; i++)
            Destroy(MSUnitList[i].gameObject);
        Destroy(SC_RPGBattleUI._instance.gameObject);
        SC_RPGBattle._instance.InitBattle();
        KillScore++;

        if (SC_RPGBattle._instance.AliveUserUnit <= 0 || SC_RPGBattle._instance.IsBoss)
        {
            if (SC_RPGBattle._instance.AliveUserUnit <= 0)
                FieldHud.transform.GetChild(3).gameObject.SetActive(true);

            GetItemSaveDummy(SC_RPGBattleUI._instance.RewordItemSet(true));
            DummySaveAndInit();
            OpenResultWindow();
        }
    }

    //배틀 보상관련
    public void GetItemSaveDummy(SC_RPGBattleUI.RewordItem rewordItem)
    {
        for (int i = 0; i < _BattleInventory.Count; i++)
        {
            if (_BattleInventory[i]["Name"].ToString() == rewordItem.ItemName)
            {
                _BattleInventory[i]["Count"] = (int)_BattleInventory[i]["Count"] + rewordItem.Count;
            }        
        }
        
    }

    public void DummySaveAndInit()
    {
        for (int i = 0; i < _RPGDummyInventory.Count; i++)
        {
            _RPGDummyInventory[i]["Count"] = (int)_RPGDummyInventory[i]["Count"] + (int)_BattleInventory[i]["Count"];
        }       
        SC_UserInfoManager._instance.InitDummyInventory(_BattleInventory);
    }

    public void OpenResultWindow()
    {
        SC_UIFunctionPool._instance.WindowOpen(PrefebFinalResultWindow, FieldHud.gameObject);
    }



    //배틀
    //몬스터 스폰
    IEnumerator EnemyUnitSpawn()
    {
        SpawnInfoInit(_MSPos, _enemyTeamSpawnPos, false);
        SC_RPGBattleUI._instance.HPbarinit();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SC_RPGBattle._instance.PhaseStart());
    }

    void SpawnInfoInit(int[] posUnit, GameObject[] arrSpawnPos, bool isUserTeam)
    {
        GameObject[] _unitPool = new GameObject[3] { SC_CharacterPool._instance.characterPool[posUnit[0]], SC_CharacterPool._instance.characterPool[posUnit[1]], SC_CharacterPool._instance.characterPool[posUnit[2]] };
        for (int i = 0; i < _unitPool.Length; i++)
        {
            if (isUserTeam)
            {
                GameObject go = Instantiate(_unitPool[i], arrSpawnPos[i].transform.position, Quaternion.identity, arrSpawnPos[i].transform);
                FindCharacterType(go, PCUnitList, (SC_PublicDefine.eUnitName)posUnit[i], i);
            }
            else
            {
                GameObject go = Instantiate(_unitPool[i], arrSpawnPos[i].transform.position, Quaternion.identity, arrSpawnPos[i].transform);
                FindCharacterType(go, MSUnitList, (SC_PublicDefine.eUnitName)posUnit[i], i);
            }
        }
    }




    void FindCharacterType(GameObject gameobj, SC_RPGUnit[] unitList, SC_PublicDefine.eUnitName name, int i)
    {
        if (name == SC_PublicDefine.eUnitName.Warrior)
            unitList[i] = gameobj.GetComponent<SC_PCWarrior>();
        else if (name == SC_PublicDefine.eUnitName.Mage)
            unitList[i] = gameobj.GetComponent<SC_PCMage>();
        else if (name == SC_PublicDefine.eUnitName.Thief)
            unitList[i] = gameobj.GetComponent<SC_PCThief>();
        else if (name == SC_PublicDefine.eUnitName.Priest)
            unitList[i] = gameobj.GetComponent<SC_PCPriest>();
        else if (name == SC_PublicDefine.eUnitName.FlyingEye)
            unitList[i] = gameobj.GetComponent<SC_MSFlyingEye>();
        else if (name == SC_PublicDefine.eUnitName.Skeleton)
            unitList[i] = gameobj.GetComponent<SC_MSSkeleton>();
        else if (name == SC_PublicDefine.eUnitName.FireSpirit)
            unitList[i] = gameobj.GetComponent<SC_MSFireSpirit>();
        else if (name == SC_PublicDefine.eUnitName.EarthSpirit)
            unitList[i] = gameobj.GetComponent<SC_MSEarthSpirit>();
        else if (name == SC_PublicDefine.eUnitName.Goblin)
            unitList[i] = gameobj.GetComponent<SC_MSGoblin>();
        else if (name == SC_PublicDefine.eUnitName.Mushroom)
            unitList[i] = gameobj.GetComponent<SC_MSMushroom>();


    }
}
