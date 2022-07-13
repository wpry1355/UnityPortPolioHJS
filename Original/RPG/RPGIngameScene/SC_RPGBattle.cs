using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_RPGBattle : MonoBehaviour
{
    static SC_RPGBattle _uniqueInstance;
    public static SC_RPGBattle _instance
    {
        get { return _uniqueInstance; }
    }

    int[] _completeSelect = new int[3];
    bool _isFirstAtt = false;

    // 생존 유닛 카운트
    int _aliveUserUnit = 3;
    int _aliveEnemyUnit = 3;

    // 턴관리
    int _nowturn = 0;

    // 애니메이션 시간 불러오기.
    float[] PCAniPlayTime = new float[3];
    float[] MSAniPlayTime = new float[3];

    bool _isBoss = false;

    public int AliveUserUnit
    {
        get { return _aliveUserUnit; }
        set { _aliveUserUnit = value; }
    }
    public int AliveEnemyUnit
    {
        get { return _aliveEnemyUnit; }
        set { _aliveEnemyUnit = value; }
    }
    public bool IsBoss
    {
        get { return _isBoss; }
        set { _isBoss = value; }
    }
    public bool IsFirstAtt
    {
        get { return _isFirstAtt; }
        set { _isFirstAtt = value; }
    }

    void Awake()
    {
        _uniqueInstance = this;
    }
    
    // 2. 턴 시작.
    public IEnumerator PhaseStart()
    {
        SC_RPGBattleUI._instance.UpdateSkillCost();
        _nowturn++;
        SC_RPGBattleUI._instance.TurnText.text = "Round " + _nowturn.ToString();
        // 0<= 스킬슬롯활성화
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            if (_completeSelect[i] == 2)
            {
                SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn = SC_RPGInGameManager._instance.PCUnitList[i]._skillTurn;
                // 여기가 슬라이더 활성 기점.
                if (SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn >= 0)
                {
                    SC_RPGBattleUI._instance.SkillSlider[i].gameObject.SetActive(true);
                    SC_RPGBattleUI._instance.UpdateSkillCost();
                }
            }
        }
        StartCoroutine(SC_RPGBattleUI._instance.Notice("Round " + _nowturn.ToString() + " Start", 1.5f));

        yield return new WaitForSeconds(2f);

        StartCoroutine(BattlePhase());
    }

    IEnumerator BattlePhase()
    {
        GetMSAniPlayTime();
        if (_isFirstAtt) // 내가 선공이 아니면 적의 턴 먼저 실행.
        {
            StartCoroutine(SC_RPGBattleUI._instance.Notice("Player Turn!!", 1.5f));
            yield return new WaitForSeconds(2f);
            StartCoroutine(SC_RPGBattleUI._instance.BtnOn());
        }
        else
        {
            StartCoroutine(SC_RPGBattleUI._instance.Notice("Enemys Turn!!", 1.5f));
            yield return new WaitForSeconds(3f);
            StartCoroutine(EnemyTurn());
        }
    }
    // 3. 유저 전투모드 선택 및 선택완료 or 도망가기.
    public void SelectBattleMode()
    {
        // UI에서 0번 1번 2번 토글 정보 가져오기
        // Front
        InputCompleteArr(SC_RPGBattleUI._instance.FrontBattleModeTG, 0);
        // Back1
        InputCompleteArr(SC_RPGBattleUI._instance.Back1BattleModeTG, 1);
        // Back2
        InputCompleteArr(SC_RPGBattleUI._instance.Back2BattleModeTG, 2);

        GetPCAniPlayTime();

        StartCoroutine(UserTurn());
    }

    void GetPCAniPlayTime()
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            SC_RPGInGameManager._instance.PCUnitList[i].FindTime(_completeSelect[i]);
            PCAniPlayTime[i] = SC_RPGInGameManager._instance.PCUnitList[i].AniPlayTime;
        }
    }
    void GetMSAniPlayTime()
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.MSUnitList.Length; i++)
        {
            int ActionNum = 0;
            switch (SC_RPGInGameManager._instance.MSUnitList[i]._nowSkillTurn > 0)
            {
                case true:
                    ActionNum = 0;
                    break;
                case false:
                    ActionNum = 2;
                    break;
            }

            SC_RPGInGameManager._instance.MSUnitList[i].FindTime(ActionNum);
            MSAniPlayTime[i] = SC_RPGInGameManager._instance.MSUnitList[i].AniPlayTime;
        }

    }


    void InputCompleteArr(Toggle[] ArrToggle, int CompleteNum)
    {
        for (int i = 0; i < ArrToggle.Length; i++)
        {
            if (ArrToggle[i].isOn)
                _completeSelect[CompleteNum] = i;
            if (_completeSelect[CompleteNum] == 1)
                SC_RPGBattleUI._instance._PCDefenseMark[CompleteNum].gameObject.SetActive(true);
        }
    }

    public void EscapeFail()
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            SC_RPGInGameManager._instance.PCUnitList[i].SelectAction(1);
        }
        StartCoroutine(SC_RPGBattleUI._instance.Notice("Escape Fail!!", 1.5f));
        StartCoroutine(EnemyTurn());
    }
    public void EscapeSuccess()
    {
        //도망 성공 기점
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
            SC_RPGInGameManager._instance.PCUnitList[i]._nowHP -= (int)(SC_RPGInGameManager._instance.PCUnitList[i]._maxHP * 0.2f);
        SC_RPGBattleUI._instance.UpdateHPbar();
        StartCoroutine(ResultWindowOpen("Escape"));
    }
    
    // 4. 턴 진행
    IEnumerator UserTurn()
    {
        for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
        {
            if (!SC_RPGInGameManager._instance.PCUnitList[i]._isdead)
            {
                if (_aliveEnemyUnit > 0)
                {
                    SC_RPGInGameManager._instance.PCUnitList[i].SelectAction(_completeSelect[i]);
                    if (_completeSelect[i] == 2)
                    {
                        SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn = SC_RPGInGameManager._instance.PCUnitList[i]._skillTurn;
                        // 여기가 슬라이더 활성 기점.
                        if (SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn >= 0)
                        {
                            SC_RPGBattleUI._instance.SkillSlider[i].gameObject.SetActive(true);
                            SC_RPGBattleUI._instance.UpdateSkillCost();
                        }

                    }
                    if (SC_RPGInGameManager._instance.PCUnitList[i]._isdead)
                        yield return new WaitForSeconds(0.1f);
                    else
                    {
                        if (_completeSelect[i] == 1)
                            yield return new WaitForSeconds(0.1f);
                        else
                        {
                            yield return new WaitForSeconds(PCAniPlayTime[i] + 2.5f);
                        }
                    }
                }
            }

        }
        // if 적군 유닛이 전부 죽었을때. BattleWin()
        if (_aliveEnemyUnit <= 0)
            //승리 기점
            StartCoroutine(ResultWindowOpen("Win"));
        else
        {
            if (_isFirstAtt)
                StartCoroutine(EnemyTurn());
            else
            {
                for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
                {
                    if (SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn > 0)
                    {
                        SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn--;
                    }
                    if (SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn <= 0)
                        SC_RPGBattleUI._instance.SkillSlider[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < SC_RPGInGameManager._instance.MSUnitList.Length; i++)
                {
                    SC_RPGInGameManager._instance.MSUnitList[i]._nowSkillTurn--;
                }
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(PhaseStart());
            }
        }

    }
    IEnumerator EnemyTurn()
    {
        SC_RPGBattleUI._instance.MSHPBarOnOff(false);
        for (int i = 0; i < SC_RPGInGameManager._instance.MSUnitList.Length; i++)
        {
            if (!SC_RPGInGameManager._instance.MSUnitList[i]._isdead)
            {
                if (_aliveUserUnit > 0)
                {
                    SC_RPGInGameManager._instance.MSUnitList[i].Target();
                    yield return new WaitForSeconds(PCAniPlayTime[i] + 2.5f);
                }
                else
                    yield return new WaitForSeconds(0.1f);
            }
        }
        for (int i = 0; i < SC_RPGBattleUI._instance._PCDefenseMark.Count; i++)
        {
            SC_RPGBattleUI._instance._PCDefenseMark[i].gameObject.SetActive(false);
        }

        //아군 전멸 기점
        if (_aliveUserUnit <= 0)
            StartCoroutine(ResultWindowOpen("Lose"));
        else
        {
            if (_isFirstAtt)
            {
                for (int i = 0; i < SC_RPGInGameManager._instance.PCUnitList.Length; i++)
                {
                    if (SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn > 0)
                        SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn--;
                    if (SC_RPGInGameManager._instance.PCUnitList[i]._nowSkillTurn <= 0)
                        SC_RPGBattleUI._instance.SkillSlider[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < SC_RPGInGameManager._instance.MSUnitList.Length; i++)
                {
                    SC_RPGInGameManager._instance.MSUnitList[i]._nowSkillTurn--;
                }
                StartCoroutine(PhaseStart());
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SC_RPGBattleUI._instance.BtnOn());
            }
        }
    }

    IEnumerator ResultWindowOpen(string Result)
    {
        SC_RPGBattleUI._instance.SetActiveResultWindow(Result);
        yield return new WaitForSeconds(5f);
    }

    public void InitBattle()
    {
        _nowturn = 0;
        _aliveEnemyUnit = 3;
    }
}
