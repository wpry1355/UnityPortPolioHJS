using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TDWarrior : SC_TDUnit
{
    [SerializeField] GameObject AttackObj;
    [SerializeField] Transform LeftAttackObjPos;
    [SerializeField] Transform RightAttackObjPos;
    void Awake()
    {
        InitData(SC_PublicDefine.eUnitName.TDWarrior);
        _aniCtrl = transform.GetComponent<Animator>();
        MyPos = new Vector2(transform.position.x, transform.position.y);
        ApplyHeroSkill();
    }

    protected override void ChangeAniToAction(SC_PublicDefine.eTDTowerActionState state)
    {
        switch (state)
        {
            case SC_PublicDefine.eTDTowerActionState.IDEL:
                break;
            case SC_PublicDefine.eTDTowerActionState.ATTACK:
                CheckAttackRotation();
                _aniCtrl.SetTrigger("Attack");
                break;
            default:
                break;
        }
    }
    protected override void AttackEvent()
    {
        if (FindAttackTarget() != null)
        {
            playAttackSound();
            if (FindAttackTarget().transform.position.x - transform.position.x > 0)
                Instantiate(AttackObj, RightAttackObjPos.position, Quaternion.identity, transform);
            else
                Instantiate(AttackObj, LeftAttackObjPos.position, Quaternion.identity, transform);
        }
    }
}
