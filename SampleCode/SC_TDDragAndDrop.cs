using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SC_TDDragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public LayerMask TileMap;
    GameObject Ring;
    GameObject RangeCircle;
    float RangeVar;
    Vector3 DefaultPos;
    Vector3Int[] ChekBuildArea = new Vector3Int[4];
    int TargetIndex;

    Tilemap[] TmpTileArea;
    Vector3Int[] TmpBuildArea;

    bool[] isCanBuildArea = new bool[4];
    Sprite DefaultImage;
    Vector3 NowTargetTile = new Vector3();
    Vector3 DstTargetTile = new Vector3();

    public void OnBeginDrag(PointerEventData eventData)
    {
        Ring = SC_TDBattleUI._Instance.sRangeRing;
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
        TargetIndex = FindImageIndex();
        DefaultImage = GetComponent<Image>().sprite;
        GetComponent<Image>().raycastTarget = false;
        GetComponent<Image>().sprite = SC_CharacterPool._instance.characterPool[TargetIndex].GetComponent<SpriteRenderer>().sprite;

        RangeVar = (int)SC_UserInfoManager._instance._TDUnitStat[FindTableIndex(TargetIndex, SC_UserInfoManager._instance._TDUnitStat)]["sRange"];
        RangeCircle = Instantiate(Ring, Vector3.zero, Quaternion.identity);
        RangeCircle.transform.localScale = RangeCircle.transform.localScale * RangeVar;
        DefaultPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        transform.position = currentPos;


        Tilemap[] CheckTileArea = new Tilemap[4];

        //레이 관련 설정, 레이어가 타일맵인 곳만 hit에 저장.
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RangeCircle.transform.position = new Vector3(MousePosition.x, MousePosition.y, 0);
        Ray2D ray = new Ray2D(MousePosition, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100, TileMap);
        Tilemap OnDragTile = hit.collider.transform.GetComponent<Tilemap>();

        // 해당 타일의 왼쪽 아래 점 좌표 반환
        int x = OnDragTile.WorldToCell(hit.point).x;
        int y = OnDragTile.WorldToCell(hit.point).y;


        // 마우스가 다음 타일로 이동시 이전 타일 색 원래대로 바꾸기.
        // 드래그 시작시에는 이전 타일이 없으니 이전타일이 있을때만 실행
        DstTargetTile = new Vector3(x, y, 0);
        if (NowTargetTile == Vector3.zero)
            NowTargetTile = new Vector3(x, y, 0);
        if (TmpBuildArea != null)
        {
            if (DstTargetTile != NowTargetTile)
            {
                ResetTileColor();
            }
        }


        //체크할 타일 설정 및 태그에 따라 해당 타일 색 변환
        fSetCheckArea(x, y, 0);
        for (int i = 0; i < ChekBuildArea.Length; i++)
        {
            Collider2D[] CheckCollider = Physics2D.OverlapBoxAll(new Vector2(ChekBuildArea[i].x + 0.5f, ChekBuildArea[i].y + 0.5f), new Vector2(0.5f, 0.5f), 0);
            isCanBuildArea[i] = true;
            for (int j = 0; j < CheckCollider.Length; j++)
            {
                if (CheckCollider[j].tag == "Wall")
                    isCanBuildArea[i] = false;
                else if (CheckCollider[j].tag == "TDMSCourse")
                    isCanBuildArea[i] = false;
                else if (CheckCollider[j].tag == "TDTower")
                    isCanBuildArea[i] = false;
                if (CheckCollider[j].transform.GetComponent<Tilemap>() != null)
                    CheckTileArea[i] = CheckCollider[j].transform.GetComponent<Tilemap>();
            }

            if (isCanBuildArea[i] == true)
            {
                CheckTileArea[i].SetTileFlags(ChekBuildArea[i], TileFlags.None);
                CheckTileArea[i].SetColor(ChekBuildArea[i], Color.green);
            }
            else
            {
                CheckTileArea[i].SetTileFlags(ChekBuildArea[i], TileFlags.None);
                CheckTileArea[i].SetColor(ChekBuildArea[i], Color.red);
            }
        }
        // 이전 타일 백업
        TmpBuildArea = ChekBuildArea;
        TmpTileArea = CheckTileArea;
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        
        GetComponent<Image>().raycastTarget = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (fCheckBuildArea(isCanBuildArea, 4) == true)
        {
            // 소환 기점
            GameObject ObjTmp= Instantiate(SC_CharacterPool._instance.characterPool[TargetIndex], new Vector3(ChekBuildArea[3].x, ChekBuildArea[3].y + 0.5f, 0), Quaternion.identity);
            SC_TDBattle._instance.BuildedTowerList.Add(ObjTmp);
            if(TargetIndex<110)
                SC_TDBattleUI._Instance.isHeroTowerBuild = true;
            SC_TDBattleUI._Instance.AddEnergy(-ObjTmp.GetComponent<SC_TDUnit>()._cost);

        }
        else
        {
            SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.CanNotBuild);
        }

        Destroy(RangeCircle);
        ResetTileColor();

        transform.position = DefaultPos;
        GetComponent<Image>().sprite = DefaultImage;
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnUp);
    }

    //체크타일 설정
    void fSetCheckArea(int x, int y, int z)
    {
        ChekBuildArea[0] = new Vector3Int(x - 1, y + 1, z);
        ChekBuildArea[1] = new Vector3Int(x, y + 1, z);
        ChekBuildArea[2] = new Vector3Int(x - 1, y, z);
        ChekBuildArea[3] = new Vector3Int(x, y, z);
    }

    // 체크된 타일 태그모두 체크
    bool fCheckBuildArea(bool[] isCanBuild, int Length)
    {
        for (int i = 0; i < Length; i++)
        {
            if (isCanBuild[i] == false)
                return false;
        }
        return true;
    }

    // 타일 색 리셋
    void ResetTileColor()
    {
        for (int i = 0; i < TmpBuildArea.Length; i++)
        {
            TmpTileArea[i].SetTileFlags(TmpBuildArea[i], TileFlags.None);
            TmpTileArea[i].SetColor(TmpBuildArea[i], Color.white);
        }
    }

    int FindImageIndex()
    {
        int Index = 0;
        for (int i = 0; i < SC_ImagePool._Instance.TotalImageCount; i++)
        {
            if (SC_ImagePool._Instance.getImage((SC_PublicDefine.eUnitName)i) == GetComponent<Image>().sprite)
            {
                Index = i;
            }
        }
        return Index;
    }

    int FindTableIndex(int FindNumber, List<Dictionary<string, object>> Table)
    {
        int Index = 0;
        for (int i =0;i<Table.Count; i++)
        {
            if ((int)Table[i]["Index"] == FindNumber)
                Index = i;
        }
        return Index;
    }
}
