using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SC_ItemInfomation : MonoBehaviour
{
    [SerializeField] Image _ItemImage;
    [SerializeField] Text _ItemName;
    [SerializeField] Text _ItemDetail;
    [SerializeField] Text _ItemCount;
    [SerializeField] Text _ItemFarmingArea;

    private void Awake()
    {

    }

    public void _InitInventory(int InventoryIndex, List<Dictionary<string, object>> ItemTable, List<Dictionary<string, object>> Inventory, Transform CanvasParent)
    {

        string ItemName;
        string ItemCount;
        string ItemDetail;
        string ItemFarmingArea;
        int ItemImageIndex;
        ItemName = Inventory[InventoryIndex]["Name"].ToString();
        ItemCount = Inventory[InventoryIndex]["Count"].ToString();

        int ItemTableIndex = SC_UserInfoManager._instance.FindItemIndex(ItemTable, ItemName);
        ItemImageIndex = (int)SC_UserInfoManager._instance._ItemTable[ItemTableIndex]["Index"]+20;

        ItemDetail = ItemTable[ItemTableIndex]["Detail"].ToString();
        ItemFarmingArea = ItemTable[ItemTableIndex]["FarmingArea"].ToString();


        _ItemName.text = ItemName;
        _ItemCount.text = "number you have : " + "<size= 50>" + ItemCount + "</size>";
        _ItemImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)ItemImageIndex);
        _ItemDetail.text = ItemDetail;
        _ItemFarmingArea.text = "ItemFarmingArea " + ItemFarmingArea;

        Instantiate(transform.gameObject, CanvasParent.position, Quaternion.identity, CanvasParent);
    }

    public void _InitCustom(List<Dictionary<string, object>> ItemTable, string ItemName,string ItemCount,string ItemDetail,string ItemFarmingArea, Transform CanvasParent,int ImageIndex)
    {
        


        _ItemName.text = ItemName;
        _ItemCount.text = "number you have : " + "<size= 50>" + ItemCount + "</size>";
        _ItemImage.sprite = SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)ImageIndex + 20);
        _ItemDetail.text = ItemDetail;
        _ItemFarmingArea.text = "ItemFarmingArea " + ItemFarmingArea;

        Instantiate(transform.gameObject, CanvasParent.position, Quaternion.identity, CanvasParent);
    }

    public void ClosePopUp()
    {
        SC_UIFunctionPool._instance.WindowClose(gameObject);
    }

}
