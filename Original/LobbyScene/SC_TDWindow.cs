using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_TDWindow : MonoBehaviour
{
    public void TDWindowClose()
    {
        SC_SoundControlManager._instance.EffectSoundPlay(SC_PublicDefine.eSoundTrack.UIClose);
        SC_UIFunctionPool._instance.WindowClose(transform.gameObject);
    }
}
