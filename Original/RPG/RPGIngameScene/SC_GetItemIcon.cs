using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_GetItemIcon : MonoBehaviour
{
    [SerializeField] Image ItemIcon;
    [SerializeField] Text ItemCount;

    public void initItemInfo(int Index, string Count, List<Dictionary<string, object>> DummyData)
    {
        ItemIcon.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)(int)DummyData[Index]["Index"] + 20);
        ItemCount.text = Count;
    }
}
