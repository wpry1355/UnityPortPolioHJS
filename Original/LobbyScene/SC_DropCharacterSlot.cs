using UnityEngine;
using UnityEngine.EventSystems;

public class SC_DropCharacterSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] int CharPosition;
    SC_HeroTabUI cHeroTabUI;


    private void Awake()
    {
        cHeroTabUI = SC_HeroTabUI._Instance;
    }
    public void OnDrop(PointerEventData eventData)
    {
        bool isDuplicate = false;
        int[] tmp = new int[3];
        int DuplicateIndex = 0;

        //중복 체크
        for (int i = 0; i < SC_UserInfoManager._instance._PCPosUnit.Length; i++)
        {
            if (SC_UserInfoManager._instance._PCPosUnit[i] == SC_HeroTabUI._Instance.DragIndex)
            {
                isDuplicate = true;
                DuplicateIndex = i;
            }
        }

        // 중복 체크에 따라 다르게 소환
        // 중복 일때 소환 기점
        if (isDuplicate)
        {
            if (DuplicateIndex == CharPosition)
                return;

            else
            {
                for (int i = 0; i < SC_UserInfoManager._instance._PCPosUnit.Length; i++)
                {
                    tmp[i] = SC_UserInfoManager._instance._PCPosUnit[i];
                }
                tmp[DuplicateIndex] = SC_UserInfoManager._instance._PCPosUnit[CharPosition];
                tmp[CharPosition] = SC_HeroTabUI._Instance.DragIndex;
                updateSpawn(tmp[0], tmp[1], tmp[2]);
            }
            cHeroTabUI._NowCharacterIndex = SC_UserInfoManager._instance._PCPosUnit[CharPosition];
            cHeroTabUI.RingColorSetting(CharPosition);
            cHeroTabUI.SelectedCharacterPanelSetting();
        }

        // 중복이 아닐때 소환 기점
        else
        {


            SC_UserInfoManager._instance._PCPosUnit[CharPosition] = SC_HeroTabUI._Instance.DragIndex;
            if (cHeroTabUI.CharSlot[CharPosition].transform.childCount == 2)
            {
                Destroy(cHeroTabUI.CharSlot[CharPosition].transform.GetChild(1).gameObject);
                Instantiate(SC_CharacterPool._instance.characterPool[SC_UserInfoManager._instance._PCPosUnit[CharPosition]], cHeroTabUI.CharSlot[CharPosition].transform);
            }
            else
                Instantiate(SC_CharacterPool._instance.characterPool[SC_UserInfoManager._instance._PCPosUnit[CharPosition]], cHeroTabUI.CharSlot[CharPosition].transform);
            cHeroTabUI._NowCharacterIndex = SC_UserInfoManager._instance._PCPosUnit[CharPosition];
            cHeroTabUI.RingColorSetting(CharPosition);
            cHeroTabUI.SelectedCharacterPanelSetting();
        }
        // FrontPosUnit,BackPosUnit1,BackPosUnit2

        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userinfodata, "FrontPosUnit", 0, SC_UserInfoManager._instance._PCPosUnit[0], "UserInfomation");
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userinfodata, "BackPosUnit1", 0, SC_UserInfoManager._instance._PCPosUnit[1], "UserInfomation");
        SC_UserInfoManager._instance.SaveData(SC_UserInfoManager._instance._userinfodata, "BackPosUnit2", 0, SC_UserInfoManager._instance._PCPosUnit[2], "UserInfomation");
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnUp);
    }
    void updateSpawn(int CharPoolIndex1, int CharPoolIndex2, int CharPoolIndex3)
    {
        Destroy(cHeroTabUI.CharSlot[0].transform.GetChild(1).gameObject);
        Destroy(cHeroTabUI.CharSlot[1].transform.GetChild(1).gameObject);
        Destroy(cHeroTabUI.CharSlot[2].transform.GetChild(1).gameObject);


        SC_UserInfoManager._instance._PCPosUnit[0] = CharPoolIndex1;
        SC_UserInfoManager._instance._PCPosUnit[1] = CharPoolIndex2;
        SC_UserInfoManager._instance._PCPosUnit[2] = CharPoolIndex3;

        Instantiate(SC_CharacterPool._instance.characterPool[SC_UserInfoManager._instance._PCPosUnit[0]], cHeroTabUI.CharSlot[0].transform);
        Instantiate(SC_CharacterPool._instance.characterPool[SC_UserInfoManager._instance._PCPosUnit[1]], cHeroTabUI.CharSlot[1].transform);
        Instantiate(SC_CharacterPool._instance.characterPool[SC_UserInfoManager._instance._PCPosUnit[2]], cHeroTabUI.CharSlot[2].transform);




    }

}
