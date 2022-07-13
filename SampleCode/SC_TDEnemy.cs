using System.Collections.Generic;
using UnityEngine;

public class SC_TDEnemy : MonoBehaviour
{
    //Stat
    string UnitName;
    int Defense;
    int SkillDefense;
    int MaxHP;
    int NowHP;
    int MoveSpeed;
    int SkillMoveSpeed;
    int LifeDamage;
    bool IsFlying;
    int Level;
    SC_PublicDefine.eSoundTrack SoundNumber;
    public Transform SpawnPos;
    public Transform GoalPos;

    protected Animator _aniCtrl;
    protected bool IsDead = false;
    protected bool IsSkill = false;


    //Navi
    Vector3 SpawnPosition = new Vector3();
    Vector3 TargetPosition = new Vector3();
    Vector3 NextMovePosition = new Vector3();
    bool isNavigation = false;
    int MovePositionIndex = 0;
    List<Vector3> ArrMovePosition = new List<Vector3>();
    SC_AINavigation cAINAvigation = new SC_AINavigation();

    //HPBar
    [SerializeField] GameObject PrefebHPbar;
    GameObject HPbar;
    RectTransform HPBarRectPos;
    float HPbarEnemyHeightVar;
    float HPbarEnemyWidthVar;
    Vector3 HPBarPos;
    public string _unitName
    {
        get { return UnitName; }
    }
    public int _defense
    {
        get { return Defense; }
    }
    public int _finishDefense
    {
        get { return Defense + SkillDefense; }
    }
    public int _maxHP
    {
        get { return MaxHP; }
    }
    public int _nowHP
    {
        get { return NowHP; }
        set { NowHP = value; }
    }
    public int _moveSpeed
    {
        get { return MoveSpeed; }
    }
    public int _finishMoveSpeed
    {
        get { return MoveSpeed + SkillMoveSpeed; }
    }
    public int _lifeDamage
    {
        get { return LifeDamage; }
    }
    public bool _isFlying
    {
        get { return IsFlying; }
    }

    public int _level
    {
        get { return Level; }
    }

    public bool _isDead
    {
        get { return IsDead; }
    }
    //OverrideFunc
    protected virtual void ChangeAniToAction(SC_PublicDefine.eTDEnemyActionState action) { }

    private void Update()
    {
        //hp바 위치 갱신
        if (HPbar != null)
        {
            HPBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + HPbarEnemyWidthVar, transform.position.y + HPbarEnemyHeightVar, 0));
            HPBarRectPos.position = HPBarPos;
        }

        if (IsFlying && !IsDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, GoalPos.position, _finishMoveSpeed * Time.deltaTime);
            if (transform.position == GoalPos.position)
                //적을 놓쳤을때 기점
                Goal();
        }
        else
        {
            if (isNavigation == true)
            {
                if (!IsSkill && !IsDead)
                    transform.position = Vector3.MoveTowards(transform.position, NextMovePosition, _finishMoveSpeed * Time.deltaTime);
                if (transform.position == TargetPosition)
                {
                    //적을 놓쳤을때 기점
                    isNavigation = false;
                    Goal();
                }
                else if (transform.position == NextMovePosition)
                {
                    MovePositionIndex++;
                    NextMovePosition = ArrMovePosition[MovePositionIndex];
                    if (transform.position.x == NextMovePosition.x)
                    {
                        return;
                    }
                    else if (transform.position.x < NextMovePosition.x)
                    {
                        gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    }
                    else
                        gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
    }
    protected void InitData(SC_PublicDefine.eUnitName Unitname)
    {

        int Target = 0;
        Level = SC_TDInGameManager._instance._stageNumIndex;

        List<Dictionary<string, object>> baseStat = SC_UserInfoManager._instance._TDEnemyStat;
        List<Dictionary<string, object>> levelVar = SC_UserInfoManager._instance._TDEnemyLevelVar;
        for (int i = 0; i < baseStat.Count; i++)
        {
            if ((int)baseStat[i]["Index"] == (int)Unitname)
            {
                Target = i;
            }
        }
        UnitName = baseStat[Target]["UnitName"].ToString();
        Defense = (int)baseStat[Target]["Defense"] + ((int)levelVar[Target]["Defense"] * (Level - 1));
        MaxHP = (int)baseStat[Target]["MaxHP"] + ((int)levelVar[Target]["MaxHP"] * (Level - 1));
        NowHP = MaxHP;
        LifeDamage = (int)baseStat[Target]["LifeDamage"];
        MoveSpeed = (int)baseStat[Target]["MoveSpeed"];
        SoundNumber = (SC_PublicDefine.eSoundTrack)Unitname;
        if ("true" == (string)baseStat[Target]["IsFlying"])
            IsFlying = true;
        else
            IsFlying = false;
        SkillCheck();
    }

    //TD 네비게이션
    public void MoveStart()
    {
        SetNavigation();
    }

    void SetNavigation()
    {
        List<Node> MoveList = new List<Node>();
        SpawnPosition = SpawnPos.position;
        TargetPosition = GoalPos.position;
        cAINAvigation.init(SpawnPosition, TargetPosition);
        cAINAvigation.PathFinding();
        MoveList = cAINAvigation.FinalNodeList;
        for (int i = 1; i < MoveList.Count; i++)
        {
            ArrMovePosition.Add(new Vector3(MoveList[i].x, MoveList[i].y, 0));
        }
        NextMovePosition = ArrMovePosition[0];
        ChangeAniToAction(SC_PublicDefine.eTDEnemyActionState.RUN);
        isNavigation = true;
    }


    //배틀 관련
    public void TakeHit(int Damage)
    {

        NowHP -= (Damage - _finishDefense);
        float fNowHP = (float)NowHP;
        float fMaxHP = (float)MaxHP;
        HPbar.SetActive(true);
        HPbar.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(fNowHP / fMaxHP * 100, HPbar.transform.GetChild(0).GetComponent<RectTransform>().rect.height);
        //데미지를 입은 시점
        // Enemy HPBar 갱신 및 DamageHud
        if (NowHP <= 0)
        {
            Destroy(HPbar);
            ChangeAniToAction(SC_PublicDefine.eTDEnemyActionState.DEAD);
            SC_TDBattle._instance._aliveEnemy--;
            SC_TDBattle._instance.EndRound();
            SC_TDBattle._instance.SpawnEnemyList.Remove(gameObject);
        }
    }


    public void SkillOn()
    {
        switch (SC_TDInGameManager._instance._selectHeroIndex)
        {
            case 2:
                SkillMoveSpeed = -(int)(MoveSpeed * 0.2f);
                break;
            case 3:
                SkillDefense = -(int)(Defense * 0.2f);
                break;
            default:
                break;
        }
    }
    public void SkillOff()
    {
        switch (SC_TDInGameManager._instance._selectHeroIndex)
        {
            case 2:
                SkillMoveSpeed = 0;
                break;
            case 3:
                SkillDefense = 0;
                break;
            default:
                break;
        }
    }
    public void SkillCheck()
    {
        if (SC_TDBattle._instance.IsBuildedHero)
            SkillOn();
        else
            SkillOff();
    }


    protected void HPbarinit(float _HPbarEnemyHeightVar, float _HPbarEnemyWidthVar, float ScaleX = 1, float ScaleY = 1)
    {
        HPbar = Instantiate(PrefebHPbar, SC_TDBattleUI._Instance.transform.GetComponent<Canvas>().transform);
        HPBarRectPos = HPbar.GetComponent<RectTransform>();
        HPBarRectPos.localScale = new Vector3(ScaleX, ScaleY, 1);
        HPbarEnemyHeightVar = _HPbarEnemyHeightVar;
        HPbarEnemyWidthVar = _HPbarEnemyWidthVar;
        HPbar.SetActive(false);
    }

    void DeadEvent()
    {
        Destroy(gameObject, 0.5f);
    }
    void Goal()
    {
        SC_TDBattle._instance._aliveEnemy--;
        SC_TDBattle._instance._life--;
        SC_TDBattleUI._Instance.UpdateLifeWindow();
        if (SC_TDBattle._instance._life >= 0)
            SC_TDBattle._instance.EndRound();
        Destroy(HPbar);
        Destroy(gameObject);
    }

    protected void PlayDeathSound()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SoundNumber);
    }
}
