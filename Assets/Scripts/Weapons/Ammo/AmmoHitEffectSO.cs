using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoHitEffect_", menuName = "Scriptable Objects/Weapon/�ӵ�������Ч")]
public class AmmoHitEffectSO : ScriptableObject
{
    #region Header �ӵ�������Ч
    [Space(10)]
    [Header("�ӵ�������Ч")]
    #endregion

    #region Tooltip
    [Tooltip("�����ӵ�����Ч����ɫ������")]
    #endregion
    public Gradient colorGradient;

    #region Tooltip
    [Tooltip("����ʱ��")]
    #endregion
    public float duration = 0.5f;

    #region Tooltip
    [Tooltip("��ʼ���Ӵ�С")]
    #endregion
    public float startParticleSize = 0.25f;

    #region Tooltip
    [Tooltip("��ʼ�����ٶ�")]
    #endregion
    public float startParticleSpeed = 3f;

    #region Tooltip
    [Tooltip("��ʼ����������")]
    #endregion
    public float startLifeTime = 0.5f;

    #region Tooltip
    [Tooltip("���ӵ��������")]
    #endregion
    public int maxdParticlesNumber = 100;

    #region Tooltip
    [Tooltip("���ӵķ�������")]
    #endregion
    public int emissionRate = 100;

    #region Tooltip
    [Tooltip("���ӱ�������")]
    #endregion
    public int burstParticleNumber = 20;

    #region Tooltip
    [Tooltip("������������")]
    #endregion
    public float effectGravity = -0.01f;

    #region Tooltip
    [Tooltip("���Ӿ���")]
    #endregion
    public Sprite effectSprite;

    #region Tooltip
    [Tooltip("���������������ٶ�")]
    #endregion
    public Vector3 velocityOverLifeTimeMin;

    #region Tooltip
    [Tooltip("��������������ٶ�")]
    #endregion
    public Vector3 velocityOverLifeTimeMax;

    #region Tooltip
    [Tooltip("�ӵ�����ЧԤ����")]
    #endregion
    public GameObject ammoHitEffectPrefab;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(duration), duration, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(startParticleSize), startParticleSize, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(startParticleSpeed), startParticleSpeed, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(startLifeTime), startLifeTime, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(maxdParticlesNumber), maxdParticlesNumber, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(emissionRate), emissionRate, true);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(burstParticleNumber), burstParticleNumber, true);
        HelpUtilities.ValidateCheckNullValues(this, nameof(ammoHitEffectPrefab), ammoHitEffectPrefab);
    }
#endif
    #endregion
}
