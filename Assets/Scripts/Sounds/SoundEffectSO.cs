using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect_",menuName = "Scriptable Objects/Sounds/SoundEffect")]
public class SoundEffectSO : ScriptableObject
{
    #region Header ��Чϸ��
    [Space(10)]
    [Header("��Чϸ��")]
    #endregion
    #region Tooltip
    [Tooltip("��Ч������")]
    #endregion
    public string soundEffectName;

    #region Tooltip
    [Tooltip("��Ч��Ԥ����")]
    #endregion
    public GameObject soundEffectPrefab;

    #region Tooltip
    [Tooltip("��Ч����Ƭ")]
    #endregion 
    public AudioClip soundEffectClip;

    #region Tooltip
    [Tooltip("��Ч���������")]
    #endregion
    [Range(0.5f, 1.5f)]
    public float soundEffectPitchRandomVariationMin = 0.8f;

    #region Tooltip
    [Tooltip("��Ч���������")]
    #endregion
    [Range(0.5f, 1.5f)]
    public float soundEffectPitchRandomVariationMax = 1.2f;

    #region Tooltip
    [Tooltip("��Ч������")]
    #endregion
    [Range(0f,1f)]
    public float soundEffectVolume = 1f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(soundEffectName),soundEffectName);
        HelpUtilities.ValidateCheckNullValues(this, nameof(soundEffectPrefab), soundEffectPrefab);
        HelpUtilities.ValidateCheckNullValues(this, nameof(soundEffectClip), soundEffectClip);
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(soundEffectPitchRandomVariationMin), soundEffectPitchRandomVariationMin, 
            nameof(soundEffectPitchRandomVariationMax), soundEffectPitchRandomVariationMax,false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(soundEffectVolume), soundEffectVolume, true);
    }

#endif
    #endregion
}
