using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PublicDefine : MonoBehaviour
{
    public enum eSoundTrack
    {
        //Default
        None = 0,

        //BGM 1~19
        TitleBGM = 1,
        LobbyBGM,
        RPGFieldBGM,
        RPGBattleBGM,
        TDBGM,
        LoadingBGM,

        //UI 20~39
        BtnClick = 20,
        BtnUp,
        UIOpen,
        UIClose,
        LevelUp,
        CanNotBuild,
        //RPG 사운드 40~99
        WarriorAttack = 40,
        ThiefAttack,
        MageAttack,
        PriestAttack,

        WarriorSkill = 50,
        ThiefSkill,
        MageSkill,
        PriestSkill,

        WarriorDeath = 60,
        ThiefDeath,
        MageDeath,
        PriestDeath,

        GoblinAttack =70,
        MushroomAttack,
        SkeletonAttack,
        FlyingEyeAttack,
        FireSpiritAttack,
        EarthSpritAttack,

        GoblinSkill = 80,
        MushroomSkill,
        SkeletonSkill,
        FlyingEyeSkill,
        FireSpiritSkill,
        EarthSpritSkill,

        GoblinDeath = 90,
        MushroomDeath,
        SkeletonDeath,
        FlyingEyeDeath,
        FireSpiritDeath,
        EarthSpritDeath,

        //TD 타워 디펜스 100~

        TDWarriorAttack = 100,
        TDThiefAttack,
        TDMageAttack,
        TDPriestAttack,

        TDNormalTowerAttack = 110,
        TDAirTowerAttack,

        TDGoblinDeath = 120,
        TDSkeletonDeath,
        TDFlyingEyeDeath,
        TDFireSpiritDeath,




    }
    public enum eDirection
    {
        Up = 0,
        Down,
        Left,
        Right
    }
    public enum eUnitName
    {
        Warrior = 0,
        Thief,
        Mage,
        Priest,



        Goblin = 10,
        Mushroom,
        Skeleton,
        FlyingEye,
        FireSpirit,
        EarthSpirit,

        TDWarrior = 100,
        TDThief = 101,
        TDMage = 102,
        TDPriest = 103,

        TDNormalTower = 110,
        TDAirTower = 111,

        TDGoblin = 120,
        TDSkeleton,
        TDFlyingEye,
        TDFireSpirit,




        None = 200
    }
    public enum eActionState
    {
        IDEL = 0,
        RUN,
        ATTACK,
        SKILL,
        HIT,
        DEAD
    }

    public enum eTDEnemyActionState
    {
        IDEL = 0,
        RUN,
        DEAD,
        SKILL
    }
    public enum eTDTowerActionState
    {
        IDEL = 0,
        ATTACK
    }
}