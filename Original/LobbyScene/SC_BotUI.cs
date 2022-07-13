using UnityEngine;

public class SC_BotUI : MonoBehaviour
{
    [SerializeField]
    public GameObject RPG_UIPanel;
    public GameObject TD_UIPanel;
    private GameObject Rpg_Canvas;
    private GameObject TD_Canvas;

    public void RpgWindowOpen()
    {
        SC_SoundControlManager._instance.BtnClickSound();
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        Instantiate(RPG_UIPanel, transform.parent.position, Quaternion.identity,transform.parent);
    }
    public void TDWindowOpen()
    {
        
        SC_SoundControlManager._instance.BtnClickSound();
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIOpen);
        Instantiate(TD_UIPanel, transform.parent.position, Quaternion.identity, transform.parent);
    }

}
