using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_MSSkeleton : SC_RPGUnit
{
    [SerializeField] GameObject _skillObject;
    [SerializeField] GameObject DefensMarkPref;
    GameObject DefenseMark;
    Transform skillPos;
    bool _isFirstSkill = true;
    void Awake()
    {
        InitData(SC_PublicDefine.eUnitName.Skeleton);
        _aniCtrl = transform.GetComponent<Animator>();
        originPos = transform.position;
        skillPos = transform.GetChild(0).GetComponent<Transform>().transform;
        RAniC = _aniCtrl.runtimeAnimatorController;

        DefenseMark = Instantiate(DefensMarkPref, SC_RPGBattleUI._instance.GetComponent<Canvas>().transform);
        RectTransform DefenseMarkRectPos = DefenseMark.GetComponent<RectTransform>();
        DefenseMarkRectPos.localScale = new Vector3(1, 1, 1);
        DefenseMarkRectPos.position = SC_RPGInGameManager._instance.BattleCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - 1f, 0));
        DefenseMark.SetActive(false);
    }

    void Update()
    {
        if (_isAction)
        {
            if (_nowSkillTurn > 0)
            {
                if (_isDefense)
                {
                    _isAction = false;
                    return;
                }
                transform.position = Vector3.MoveTowards(transform.position, attPos, Time.deltaTime * 10f);
                if (attPos == transform.position)
                {
                    if (_nowSkillTurn > 0)
                        ChangeAniToAction(SC_PublicDefine.eActionState.ATTACK);
                    _isAction = false;
                }
            }
            else
            {
                ChangeAniToAction(SC_PublicDefine.eActionState.SKILL);
                _nowSkillTurn = _skillTurn;
                _isAction = false;
            }
        }
    }

    public override void ChangeAniToAction(SC_PublicDefine.eActionState state)
    {
        switch (state)
        {
            case SC_PublicDefine.eActionState.IDEL:
                _aniCtrl.SetBool("IsRun", false);
                break;
            case SC_PublicDefine.eActionState.RUN:
                _aniCtrl.SetBool("IsRun", true);
                break;
            case SC_PublicDefine.eActionState.ATTACK:
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.SkeletonAttack);
                _aniCtrl.SetTrigger("Attack");
                break;
            case SC_PublicDefine.eActionState.SKILL:
                if (!_isFirstSkill)
                {
                    SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.SkeletonSkill);
                    _aniCtrl.SetTrigger("DToA");
                }
                else
                {
                    DefenseMark.SetActive(true);
                    _aniCtrl.SetBool("IsDefense", true);
                    _isFirstSkill = false;
                }
                break;
            case SC_PublicDefine.eActionState.HIT:
                _aniCtrl.SetTrigger("Hit");
                break;
            case SC_PublicDefine.eActionState.DEAD:
                _isdead = true;
                SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.SkeletonDeath);
                _aniCtrl.SetBool("IsDead", true);
                break;
        }
    }
    public override void FindTime(int SelectAction)
    {
        switch (SelectAction)
        {
            case 0:
                FindAniPlayTime("MSSkeleton_ATTACK");
                break;
            case 2:
                if (!_isFirstSkill)
                {
                    FindAniPlayTime("MSSkeleton_DToA");
                }
                break;
            default:
                AniPlayTime = 0.5f;
                break;
        }
    }
    protected override void OverrideSkillPos()
    {
        if (!_isDefense)
            DefenseMode();
        attPos = transform.position;

    }

    void ShootSkillObject()
    {
        Instantiate(_skillObject, skillPos.position, Quaternion.identity, transform);
    }
}
