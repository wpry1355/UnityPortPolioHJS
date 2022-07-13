using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_FinalResultWindow : MonoBehaviour
{
    [SerializeField] GameObject prefebGetItemIcon;
    [SerializeField] Text GoldTxt;
    [SerializeField] GameObject[] ItemSlot = new GameObject[5];
    Text ResultTxt;
    int EarnedGold = 0;
    public List<int> FindIndex = new List<int>();
    List<Dictionary<string, object>> DummyData;


    void Awake()
    {
        Time.timeScale = 0.5f;
        ResultTxt = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        if (SC_UserInfoManager._instance._isPlayingRPG && !SC_UserInfoManager._instance._isPlayingTD)
        {
            DummyData = SC_RPGInGameManager._instance._RPGDummyInventory;
            if (SC_RPGBattle._instance.AliveUserUnit > 0)
            {
                ResultTxt.text = "Clear!!";
                ResultTxt.color = Color.blue;
                EarnedGold = (int)(Random.Range(0.8f, 1.2f) * 100 * (SC_RPGInGameManager._instance.MapVar + 1)) * SC_RPGInGameManager._instance.KillScore;
            }
            else
            {
                ResultTxt.text = "Fail";
                ResultTxt.color = Color.red;
            }
            GoldTxt.text = EarnedGold.ToString();
        }
        else if (!SC_UserInfoManager._instance._isPlayingRPG && SC_UserInfoManager._instance._isPlayingTD)
        {
            DummyData = SC_TDInGameManager._instance._TDDummyInventory;
            if (SC_TDBattle._instance.Life > 0)
            {
                ResultTxt.text = "Clear!!";
                ResultTxt.color = Color.blue;
                EarnedGold = (SC_TDInGameManager._instance._stageNumIndex + 1) * Random.Range(500, 601);
            }
            else
            {
                ResultTxt.text = "Fail";
                ResultTxt.color = Color.red;
            }
            GoldTxt.text = EarnedGold.ToString();
        }

        SC_UserInfoManager._instance.AddItemIndex(DummyData, FindIndex);

        InitItemData(FindIndex);
    }

    public void ExitBtn()
    {
        SC_UserInfoManager._instance.AddGold(EarnedGold);
        SC_UserInfoManager._instance.InventoryUpdate(DummyData);
        SC_UserInfoManager._instance.InitDummyInventory(DummyData);
        SC_UserInfoManager._instance._isPlayingRPG = false;
        SC_UserInfoManager._instance._isPlayingTD = false;
        Time.timeScale = 1;
        SC_UIFunctionPool._instance.loadScene("LobbyScene");
    }



    void InitItemData(List<int> _findIndex)
    {
        GameObject tmp = new GameObject();
        for (int i = 0; i < _findIndex.Count; i++)
        {
            tmp = Instantiate(prefebGetItemIcon, ItemSlot[i].transform);
            tmp.GetComponent<SC_GetItemIcon>().initItemInfo(_findIndex[i], DummyData[_findIndex[i]]["Count"].ToString(),DummyData);
        }
    }

}