using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TDFireSpirit : SC_TDEnemy
{
    void Awake()
    {
        InitData(SC_PublicDefine.eUnitName.TDFireSpirit);
        HPbarinit(3.5f, 1,2.5f,1.2f);
        _aniCtrl = transform.GetComponent<Animator>();
    }
    protected override void ChangeAniToAction(SC_PublicDefine.eTDEnemyActionState action)
    {
        switch (action)
        {
            case SC_PublicDefine.eTDEnemyActionState.IDEL:
                _aniCtrl.SetBool("IsRun", false);
                break;
            case SC_PublicDefine.eTDEnemyActionState.RUN:
                _aniCtrl.SetBool("IsRun", true);
                break;
            case SC_PublicDefine.eTDEnemyActionState.DEAD:
                PlayDeathSound();
                IsDead = true;
                _aniCtrl.SetBool("IsDead", true);
                
                break;
            default:
                break;
        }
    }
}
