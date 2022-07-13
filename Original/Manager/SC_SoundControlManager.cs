using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SoundControlManager : MonoBehaviour
{
    [SerializeField] AudioClip[] SoundPool;

    static SC_SoundControlManager _uniqueInstance;
    float BGMVolume = 0f;
    float EffectSoundVolume = 0f;

    float SaveBGMMuteVolume;
    float SaveEffectMuteVolume;

    public float _BGMVolum { get { return BGMVolume;} }
    public float _EffectSoundVolume { get { return EffectSoundVolume; } }

    public float _saveBGMMuteVolume { get { return SaveBGMMuteVolume; } set { SaveBGMMuteVolume = value; } }
    public float _saveEffectMuteVolume { get { return SaveEffectMuteVolume; } set { SaveEffectMuteVolume = value; } }


    public GameObject NowPlayBGM;
    List<GameObject> NowEffectSoundPlayList;
    public static SC_SoundControlManager _instance
    {
        get { return _uniqueInstance; }
    }

    void Awake()
    {
        
        _uniqueInstance = this;
        DontDestroyOnLoad(gameObject);
        SoundInit();
        BGMSoundPlay(SC_PublicDefine.eSoundTrack.TitleBGM);
    }

    void SoundInit()
    {
        NowEffectSoundPlayList = new List<GameObject>();
        BGMVolume = 0.5f;
        EffectSoundVolume = 0.5f;
        SaveBGMMuteVolume = BGMVolume;
        SaveEffectMuteVolume = EffectSoundVolume;
    }

    //효과음 관련
    public void EffectSoundPlay(SC_PublicDefine.eSoundTrack SoundNumber)
    {
        GameObject goTmp = new GameObject("EffectSoundPlayer");
        NowEffectSoundPlayList.Add(goTmp);
        goTmp.AddComponent<AudioSource>();
        goTmp.GetComponent<AudioSource>().volume = EffectSoundVolume;
        goTmp.GetComponent<AudioSource>().clip = GetAudioSource((int)SoundNumber);
        goTmp.GetComponent<AudioSource>().Play();
        StartCoroutine(EffectSoundReset(goTmp, goTmp.GetComponent<AudioSource>().clip.length));
    }

    IEnumerator EffectSoundReset(GameObject EffectSoundGameObject, float SoundLength)
    {
        yield return new WaitForSeconds(SoundLength);
        NowEffectSoundPlayList.Remove(EffectSoundGameObject);
        Destroy(EffectSoundGameObject);
    }

    public void EffectSoundVolumeSetting(float SoundVolume)
    {
        EffectSoundVolume = SoundVolume;
        for (int i = 0; i < NowEffectSoundPlayList.Count; i++)
            NowEffectSoundPlayList[i].GetComponent<AudioSource>().volume = EffectSoundVolume;
    }

    //BGM 관련
    public void BGMSoundPlay(SC_PublicDefine.eSoundTrack SoundNumber)
    {
        NowPlayBGM = new GameObject("BGMSoundPlayer");
        AudioSource NowPlayBGMAudioSource= NowPlayBGM.AddComponent<AudioSource>();
        NowPlayBGMAudioSource.volume = BGMVolume;
        NowPlayBGMAudioSource.clip = GetAudioSource((int)SoundNumber);
        NowPlayBGMAudioSource.loop = true;
        NowPlayBGMAudioSource.Play();
    }
    public void BGMVolumeSetting(float SoundVolume)
    {
        BGMVolume = SoundVolume;
        if(NowPlayBGM != null)
            NowPlayBGM.GetComponent<AudioSource>().volume = BGMVolume;
    }




    public AudioClip GetAudioSource(int Index)
    {
        if (SoundPool[Index] != null)
            return SoundPool[Index];
        else
            return SoundPool[0];
    }

    //자주쓰는 사운드
    public void BtnClickSound()
    {
        EffectSoundPlay(SC_PublicDefine.eSoundTrack.BtnClick);
    }
}
