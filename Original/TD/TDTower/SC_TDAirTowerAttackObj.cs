using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TDAirTowerAttackObj : MonoBehaviour
{
    SC_TDAirTower parent;
    Animator _aniCtrl;
    Vector3 targetPos;
    bool isBoom = false;
    void Awake()
    {
        parent = transform.parent.GetComponent<SC_TDAirTower>();
        _aniCtrl = transform.GetComponent<Animator>();
        targetPos = parent.FindAttackTarget().transform.position;
        if (targetPos.x - transform.position.x < 0)
            transform.GetComponent<SpriteRenderer>().flipX = true;
        else
            transform.GetComponent<SpriteRenderer>().flipX = false;
    }
    

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 10);
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            if (!isBoom)
            {
                _aniCtrl.SetTrigger("Boom");
                isBoom = true;
            }
        }
    }
    void BoomEvent()
    {
        Destroy(gameObject);
    }
}
