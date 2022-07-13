using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SC_TDBattle : MonoBehaviour
{
    [SerializeField] public Transform[] SpawnPosition;
    [SerializeField] public Transform[] GoalPosition;
    [SerializeField] public int LastRound;
    public List<GameObject> BuildedTowerList = new List<GameObject>();
    public List<GameObject> SpawnEnemyList = new List<GameObject>();
    public int Life = 15;
    public int Round = 1;
    public float RoundTime = 0;
    const float InitRoundTime = 3;
    public int AliveEnemy = 0;
    public int WaitSpawnCount = 0;
    public bool IsBuildedHero = false;
    public int _aliveEnemy { get { return AliveEnemy; } set { AliveEnemy = value; } }
    public int _life { get { return Life; } set { Life = value; } }

    int StageNum;
    struct stEnemyInfo
    {
        public string EnemyName;
        public Transform SpawnPos;
        public Transform GoalPos;
        public int SpawnCount;
    }

    List<stEnemyInfo> SpawnInfo = new List<stEnemyInfo>();

    static SC_TDBattle _uniqueInstance;
    public static SC_TDBattle _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        _uniqueInstance = this;
        StageNum = SC_TDInGameManager._instance._stageNumIndex;
        LastRound = (int)SC_TDInGameManager._instance._TDStageInfoData[StageNum]["MaxRound"];
    }

    public void GameStart()
    {
        SC_SoundControlManager._instance.BGMSoundPlay(SC_PublicDefine.eSoundTrack.TDBGM);
        StartCoroutine(InitRound());
    }
    void Update()
    {
        //타이머
        if (RoundTime > 0)
            RoundTime -= Time.deltaTime;
        SC_TDBattleUI._Instance.UpdateWaitTimeWindow();
    }

    // 1.InitGame
    IEnumerator InitRound()
    {
        RoundTime = InitRoundTime;
        yield return new WaitForSeconds(RoundTime);
        SetEnemyInfo();
        StartCoroutine(StartRound());
    }
    // 2.RoundStart
    IEnumerator StartRound()
    {
        SC_TDBattleUI._Instance.NoticePopUp(Round + "Round");
        
        yield return new WaitForSeconds(1.5f);
        SC_TDBattleUI._Instance.UpdateRoundWindow();
        StartCoroutine(SpawnEnemy(SpawnInfo));
    }

    // 3. SpawnEnemy
    IEnumerator SpawnEnemy(List<stEnemyInfo> EnemyInfoList)
    {
        for (int i = 0; i < EnemyInfoList.Count; i++)
        {
            for (int j = 0; j < EnemyInfoList[i].SpawnCount; j++)
            {
                GameObject go;
                go = Instantiate(SC_CharacterPool._instance.characterPool[(int)(SC_PublicDefine.eUnitName)Enum.Parse(typeof(SC_PublicDefine.eUnitName), EnemyInfoList[i].EnemyName)], EnemyInfoList[i].SpawnPos);
                go.GetComponent<SC_TDEnemy>().SpawnPos = EnemyInfoList[i].SpawnPos;
                go.GetComponent<SC_TDEnemy>().GoalPos = EnemyInfoList[i].GoalPos;
                go.GetComponent<SC_TDEnemy>().SkillCheck();
                go.GetComponent<SC_TDEnemy>().MoveStart();
                SpawnEnemyList.Add(go);
                AliveEnemy++;
                WaitSpawnCount--;
                SC_TDBattleUI._Instance.UpdateWaitSummonVarWindow();
                if (EnemyInfoList[i].SpawnCount == j + 1)
                    yield return new WaitForSeconds(1f);
                yield return new WaitForSeconds(0.5f);
            }
        }
        SpawnInfo.Clear();
    }

    public void EndRound() // 몬스터 처치시 함수호출.
    {

        if (Round != LastRound && AliveEnemy <= 0 && WaitSpawnCount <= 0)
        {
            Round++;
            SC_TDBattleUI._Instance.AddEnergy((int)SC_TDInGameManager._instance._TDStageInfoData[StageNum]["RoundAddEnergy"]);
            StartCoroutine(InitRound());
        }
        else
        {
            if (Life <= 0)
            {
                // 스테이지 패배
                SC_TDBattleUI._Instance.OpenResultWindow(false);
            }
            else if (Round == LastRound && AliveEnemy <= 0 && WaitSpawnCount <= 0)
            {
                // 스테이지 승리.

                SC_TDBattleUI._Instance.OpenResultWindow(true);
            }
        }

    }

    //소환 정보 Set 함수
    void SetEnemyInfo()
    {
        List<Dictionary<string, object>> data = SC_TDInGameManager._instance._StageAndRoundData;
        stEnemyInfo tst = new stEnemyInfo();
        for (int i = 0; i < data.Count ; i++)
        {
            if ((int)data[i]["Stage"] == StageNum+1 && (int)data[i]["Round"] == Round)
            {
                tst.EnemyName = (string)data[i]["Name"];
                tst.SpawnCount = (int)data[i]["Count"];
                tst.SpawnPos = SpawnPosition[(int)data[i]["SpawnPosition"]];
                tst.GoalPos = GoalPosition[(int)data[i]["GoalPosition"]];
                WaitSpawnCount += tst.SpawnCount;
                SpawnInfo.Add(tst);
            }
        }
        SC_TDBattleUI._Instance.UpdateWaitSummonVarWindow();
    }
}
