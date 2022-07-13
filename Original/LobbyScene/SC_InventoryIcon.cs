using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_InventoryIcon : MonoBehaviour
{
    [SerializeField] int Index;
    [SerializeField] GameObject prefebItemPopUp;
    int ImageIndex;
    string ItemName;

    private void Awake()
    {
        UpdateInvetory();
    }

    private void Update()
    {
        UpdateInvetory();
    }

    public void UpdateInvetory()
    {
        ItemName = SC_UserInfoManager._instance._userInventory[Index]["Name"].ToString();
        if (ItemName != "null")
        {
            for (int i = 0; i < SC_UserInfoManager._instance._ItemTable.Count; i++)
            {
                if (SC_UserInfoManager._instance._ItemTable[i]["Name"].ToString() == ItemName)
                {
                    ImageIndex = (int)SC_UserInfoManager._instance._ItemTable[i]["Index"];
                    break;
                }
            }
            transform.GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)ImageIndex + 20);

            if (SC_UserInfoManager._instance._userInventory[Index]["Count"].ToString() != "null")
                transform.GetChild(1).GetComponent<Text>().text = SC_UserInfoManager._instance._userInventory[Index]["Count"].ToString();
            else
                transform.GetChild(1).GetComponent<Text>().text = " ";
        }
        else
        {
            transform.GetChild(1).GetComponent<Text>().text = " ";
            transform.GetChild(0).GetComponent<Image>().sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)29);
        }
    }

    public void OnTouchEvent()
    {
        if (SC_UserInfoManager._instance._userInventory[Index]["Name"].ToString() != "null")
        {
            SC_SoundControlManager._instance.BtnClickSound();
            prefebItemPopUp.GetComponent<SC_ItemInfomation>()._InitInventory(Index, SC_UserInfoManager._instance._ItemTable, SC_UserInfoManager._instance._userInventory, gameObject.transform.parent.parent.parent.parent);
        }

    }


}
