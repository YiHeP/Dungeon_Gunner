using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MusicTrack_",menuName = "Scriptable Objects/Sounds/������Ŀ")]
public class MusicTrackSO : ScriptableObject
{
    #region Header ������Ŀϸ��
    [Space(10)]
    [Header("������Ŀϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("������Ŀ����")]
    #endregion
    public string musicName;

    #region Tooltip
    [Tooltip("������Ŀ��Ƭ")]
    #endregion
    public AudioClip audioClip;

    #region Tooltip
    [Tooltip("������Ŀ����")]
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
