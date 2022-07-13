using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TDAirTower : SC_TDUnit
{
    [SerializeField] GameObject AttackObj;
    [SerializeField] Transform AttackObjPos;
    void Awake()
    {
        InitData(SC_PublicDefine.eUnitName.TDAirTower);
        _aniCtrl = transform.GetComponent<Animator>();
        MyPos = new Vector2(transform.position.x, transform.position.y);
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
            Instantiate(AttackObj, AttackObjPos.position, Quaternion.identity, transform);
        }
    }


}
