using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class SC_CharacterController : MonoBehaviour
{
    static SC_CharacterController _uniqueInstance;

    [SerializeField] Sprite[] arrIdleImage;


    public Vector3 m_Direction;

    public bool IsDown;
    public bool IsLeft;
    public bool IsRight;
    public bool IsUp;
    public bool IsCheckStay;
    public bool IsfrontEnemy;
    public bool IsMoving;

    private Animator m_Animator;
    private SC_EnemyController TargetEnemy;
    private Collider2D[] arrCheckEnemy;
    private Vector3 DstPosition;
    private Vector3 StayDirection;

    private string StayAnimName;
    private bool IsStayDirectionCheck;
    private float TimeCheck;


    public static SC_CharacterController _Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
        m_Animator = transform.GetComponent<Animator>();
        transform.GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;

    }
    void Update()
    {
        if (SC_RPGInGameManager._instance.IsBattle == false)
        {
            if (IsCheckStay == true)
            {
                TimeCheck += Time.deltaTime;
                if (TimeCheck > 0.3f)
                {

                    CharacterMove(StayDirection, IsStayDirectionCheck, StayAnimName);
                    TimeCheck = 0;
                }
            }

            //에디터에서 테스트
#if UNITY_EDITOR
            if (IsMoving == false)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    m_Direction = Vector3.up;
                    CharacterMove(m_Direction, IsUp, "UpMove");
                    StayInitInfo(m_Direction, IsUp, "UpMove");
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    m_Direction = Vector3.down;
                    CharacterMove(m_Direction, IsDown, "DownMove");
                    StayInitInfo(m_Direction, IsDown, "DownMove");
                }


                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    m_Direction = Vector3.left;
                    CharacterMove(m_Direction, IsLeft, "LeftMove");
                    StayInitInfo(m_Direction, IsLeft, "LeftMove");
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    m_Direction = Vector3.right;
                    CharacterMove(m_Direction, IsRight, "RightMove");
                    StayInitInfo(m_Direction, IsRight, "RightMove");
                }
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                IsCheckStay = false;
                StayInitInfo(Vector3.zero, false, null);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                IsCheckStay = false;
                StayInitInfo(Vector3.zero, false, null);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                IsCheckStay = false;
                StayInitInfo(Vector3.zero, false, null);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                IsCheckStay = false;
                StayInitInfo(Vector3.zero, false, null);
            }
#endif
        }

    }


    //Move 관련
    private bool MoveCheck(Collider2D[] MoveJudge, Vector3 Direction)
    {
        foreach (Collider2D collider in MoveJudge)
        {
            if (collider.tag == "Wall")
            {
                return true;
            }
            else if(collider.tag == "Enemy")
            {
                return true;
            }
            float x;
            float y;
            x = collider.transform.GetComponent<Tilemap>().WorldToCell(transform.localPosition + Direction).x + 0.5f;
            y = collider.transform.GetComponent<Tilemap>().WorldToCell(transform.localPosition + Direction).y + 0.5f;
            DstPosition = new Vector3(x, y, 0);
        }
        return false;
    }


    public void CharacterMove(Vector3 Direction, bool DirectionCheck, string AnimTriggerName)
    {
        Collider2D[] MoveJudge = Physics2D.OverlapBoxAll(transform.localPosition + Direction, new Vector2(0.8f, 0.8f), 0);

        DirectionCheck = MoveCheck(MoveJudge, Direction);

        m_Animator.SetTrigger(AnimTriggerName);
        if (DirectionCheck == false)
        {
            //RPG 플레이어 심볼 이동 기점
            transform.position = DstPosition;
            transform.GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;
        }
        IsCheckStay = true;
        IsMoving = true;
        CheckEnemy();
    }


    public void StayInitInfo(Vector3 Direction, bool IsDirectionCheck, string AnimName)
    {
        StayDirection = Direction;
        IsStayDirectionCheck = IsDirectionCheck;
        StayAnimName = AnimName;
        TimeCheck = 0;
    }

    public void MovingReset()
    {
        IsMoving = false;
    }

    public void FirstMove(Vector3 Direction, bool DirectionCheck, string AnimTriggerName)
    {
        CharacterMove(Direction, DirectionCheck, AnimTriggerName);
        StayInitInfo(Direction, DirectionCheck, AnimTriggerName);
    }


    //인터렉트 관련
    private void CheckEnemy()
    {
        bool Tmp = false;
        arrCheckEnemy = Physics2D.OverlapBoxAll(transform.localPosition + m_Direction, new Vector2(0.8f, 0.8f), 0);
        foreach (Collider2D collider in arrCheckEnemy)
        {
            if (collider.tag == "Enemy")
            {
                Tmp = true;
                TargetEnemy = collider.transform.GetComponent<SC_EnemyController>();
            }
        }
        IsfrontEnemy = Tmp;
    }

    public void EnemyInteract()
    {
        SC_RPGBattle._instance.IsFirstAtt = true;
        IsCheckStay = false;
        SC_RPGInGameManager._instance.BattleIn(TargetEnemy.MonsterInfo);
        Destroy(TargetEnemy.gameObject);
    }
}
