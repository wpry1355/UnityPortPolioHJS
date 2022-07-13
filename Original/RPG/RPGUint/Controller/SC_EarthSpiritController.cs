using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_EarthSpiritController : SC_EnemyController
{
    private void Awake()
    {
        int Spawn1 = Random.Range(10, 14);
        float Spawn2 = Random.Range(10, 14);
        Spawn2 = Spawn2 / 4 + 10;


        MonsterInfo = new int[3] { 15, Spawn1, (int)Spawn2 };
        IsBoss = true;
        m_animator = transform.GetComponent<Animator>();
    }
    public override void MoveAnimation(Vector3 Dst, Vector3 Src)
    {
        Vector3 Var = Dst - Src;
        if (Var == Vector3.up)
        { m_animator.SetTrigger("UpMove"); }
        else if (Var == Vector3.down)
        { m_animator.SetTrigger("DownMove"); }
        else if (Var == Vector3.right)
        { m_animator.SetTrigger("RightMove"); }
        else if (Var == Vector3.left)
        { m_animator.SetTrigger("LeftMove"); }
    }
}
