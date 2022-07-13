using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_RPGWindow : MonoBehaviour
{
    public void RPGWindowClose()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }
}
