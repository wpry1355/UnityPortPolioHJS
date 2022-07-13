using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_GoblinSkill : MonoBehaviour
{
    SC_RPGUnit parent;
    Animator _aniCtrl;
    Vector3 targetPos;
    bool isBoom = false;
    void Awake()
    {
        parent = transform.parent.GetComponent<SC_RPGUnit>();
        _aniCtrl = transform.GetComponent<Animator>();
        targetPos = SC_RPGInGameManager._instance.PCUnitList[parent.targetArrNum].gameObject.transform.position + new Vector3(0, 2, 0);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 10);
        if (transform.position == targetPos)
            if (!isBoom)
            {
                _aniCtrl.SetTrigger("Boom");
                isBoom = true;
            }
    }

    IEnumerator BoomEvent()
    {
        parent.SkillToDamage();
        yield return new WaitForSeconds(0.1f);
        parent.ChangeAniToAction(SC_PublicDefine.eActionState.IDEL);
        parent.transform.position = parent.originPos;
        Destroy(gameObject);
    }
}
