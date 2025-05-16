using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MusicTrack_",menuName = "Scriptable Objects/Sounds/ÒôÀÖÇúÄ¿")]
public class MusicTrackSO : ScriptableObject
{
    #region Header ÒôÀÖÇúÄ¿Ï¸½Ú
    [Space(10)]
    [Header("ÒôÀÖÇúÄ¿Ï¸½Ú")]
    #endregion

    #region Tooltip
    [Tooltip("ÒôÀÖÇúÄ¿Ãû³Æ")]
    #endregion
    public string musicName;

    #region Tooltip
    [Tooltip("ÒôÀÖÇúÄ¿ÇĞÆ¬")]
    #endregion
    public AudioClip audioClip;

    #region Tooltip
    [Tooltip("ÒôÀÖÇúÄ¿Ãû³Æ")]
    #endregion
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this, nameof(musicName), musicName);
        HelpUtilities.ValidateCheckNullValues(this, nameof(audioClip), audioClip);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(musicVolume), musicVolume,true);
    }

#endif
    #endregion
}
