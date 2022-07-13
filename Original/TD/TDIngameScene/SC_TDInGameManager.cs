using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SC_TDInGameManager : MonoBehaviour
{
    [SerializeField] GameObject[] Map;
    int SelectHeroIndex;
    int StageNumIndex = 0;

    public int _stageNumIndex { get { return StageNumIndex; } set { StageNumIndex = value; } }
    public int _selectHeroIndex { get { return SelectHeroIndex; } set { SelectHeroIndex = value; } }
    public List<Dictionary<string, object>> _TDStageInfoData;
    public List<Dictionary<string, object>> _StageAndRoundData;
    public List<Dictionary<string, object>> _TDDummyInventory;

    static SC_TDInGameManager _uniqueinstance;
    public static SC_TDInGameManager _instance
    {
        get { return _uniqueinstance; }
    }

    private void Awake()
    {
        _uniqueinstance = this;
        _TDStageInfoData = SC_CSVReader.Read("TDStageInfomation");
        _StageAndRoundData = SC_CSVReader.Read("StageAndRoundData");
        _TDDummyInventory = SC_CSVReader.Read("TDDummyInventory");
        StageNumIndex = SC_UserInfoManager._instance._SelectedStageNumber;
        Instantiate(Map[StageNumIndex]);

    }
}
