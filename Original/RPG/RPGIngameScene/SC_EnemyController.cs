using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class SC_EnemyController : MonoBehaviour
{
    private Vector3 DstPosition;

    private List<Vector3> m_arrDirection;
    protected int StageVar;
    protected List<Dictionary<string, object>> _SpawnData;
    private bool IsFindPlayer;
    private int SpawnVar;
    public int[] MonsterInfo = new int[3];
    private float MoveCoolTime;
    private float MoveTerm;
    private float normalMoveTerm;
    Vector3 DstPos = new Vector3();
    SC_AINavigation cNavigation = new SC_AINavigation();
    protected Animator m_animator;
    protected bool IsBoss;

    // override Func
    public virtual void MoveAnimation(Vector3 Dst, Vector3 Src) { }

    void Start()
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;
        normalMoveTerm = 1.0f;
        MoveTerm = normalMoveTerm;
    }


    public void initController()
    {
        m_animator = transform.GetComponent<Animator>();
    }

    void Update()
    {
        if (SC_RPGInGameManager._instance.IsBattle == false)
        {
            if (MonsterInfo[0] < 14)
            {
                if (IsFindPlayer == false)
                {
                    MoveCoolTime += Time.deltaTime;
                    if (MoveCoolTime > MoveTerm)
                    {
                        EnemyMove();
                        MoveCoolTime = 0;
                    }
                }
                else
                {
                    MoveCoolTime += Time.deltaTime;
                    if (MoveCoolTime > 1.0f)
                    {
                        cNavigation.init(transform.position, SC_CharacterControler._Instance.transform.position);
                        if (cNavigation.PathFinding() != null)
                        {
                            DstPos = new Vector3(cNavigation.PathFinding().x, cNavigation.PathFinding().y, 0);
                            MoveAnimation(DstPos, transform.position);
                            //전투 돌입 기점
                            if (SC_CharacterControler._Instance.transform.position == DstPos)
                            {
                                SC_RPGInGameManager._instance.BattleIn(MonsterInfo);
                                SC_RPGBattle._instance.IsBoss = IsBoss;
                                int RdVar = Random.Range(0, 2);
                                if (RdVar == 0)
                                    SC_RPGBattle._instance.IsFirstAtt = true;
                                else
                                    SC_RPGBattle._instance.IsFirstAtt = false;
                                SC_CharacterControler._Instance.IsCheckStay = false;
                                Destroy(gameObject);
                            }
                            transform.position = DstPos;
                            transform.GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;

                        }
                        else
                        {
                            IsFindPlayer = false;
                        }
                        MoveCoolTime = 0;
                    }
                }
            }
        }

    }

    private bool MoveCheck(Vector3 Direction)
    {

        Collider2D[] CheckCharacter = Physics2D.OverlapBoxAll(transform.localPosition + Direction, new Vector2(0.8f, 0.8f), 0);
        foreach (Collider2D collider in CheckCharacter)
        {
            if (collider.tag == "Wall")
            {
                return true;
            }
            else if (collider.tag == "Player")
            {
                return true;
            }
            else if (collider.tag == "EnemyArea")
            {
                return true;
            }

        }
        return false;
    }

    private void EnemyMove()
    {
        initDirectionList();
        for (int i = 0; i < m_arrDirection.Count; i++)
        {
            if (MoveCheck(m_arrDirection[i]) == true)
            {
                m_arrDirection.RemoveAt(i);
            }
        }

        int RandomIndex = 0;

        RandomIndex = RandomIndex + Random.Range(0, m_arrDirection.Count) % m_arrDirection.Count;
        MoveAnimation(m_arrDirection[RandomIndex], Vector3.zero);
        Move(m_arrDirection[RandomIndex]);

        FindPlayer(m_arrDirection[RandomIndex]);

    }


    void Move(Vector3 Direction)
    {
        DetPositionSet(Direction, null);
        if (MoveCheck(Direction) == false)
        {
            //RPG 몬스터 심볼 이동 기점
            transform.position = DstPosition;
        }
    }

    void DetPositionSet(Vector3 Direction, string AnimationName)
    {

        Collider2D[] CheckCharacter = Physics2D.OverlapBoxAll(transform.localPosition + Direction, new Vector2(0.8f, 0.8f), 0);
        int CheckCharacterLength = CheckCharacter.Length;
        Collider2D[] tmp = new Collider2D[CheckCharacterLength];
        int Index = 0;
        for (int i = 0; i < CheckCharacter.Length; i++)
        {
            if (CheckCharacter[i].tag != "Player")
            {
                tmp[Index] = CheckCharacter[i];
                Index++;
            }
        }
        CheckCharacter = tmp;
        float x;
        float y;
        x = CheckCharacter[0].transform.GetComponent<Tilemap>().WorldToCell(transform.localPosition + Direction).x + 0.5f;
        y = CheckCharacter[0].transform.GetComponent<Tilemap>().WorldToCell(transform.localPosition + Direction).y + 0.5f;
        DstPosition = new Vector3(x, y, 0);


    }
    void initDirectionList()
    {
        m_arrDirection = new List<Vector3> { Vector3.up, Vector3.down, Vector3.right, Vector3.left };
    }


    void FindPlayer(Vector3 Direction)
    {

        Collider2D[] CheckCharacter = Physics2D.OverlapBoxAll(transform.localPosition + Direction, new Vector2(10.8f, 10.8f), 0);
        foreach (Collider2D collider in CheckCharacter)
        {
            if (collider.tag == "Player")
            {
                IsFindPlayer = true;
            }
        }
    }



}

