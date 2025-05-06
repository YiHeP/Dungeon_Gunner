using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoHitEffect_", menuName = "Scriptable Objects/Weapon/子弹命中特效")]
public class AmmoHitEffectSO : ScriptableObject
{
    #region Header 子弹命中特效
    [Space(10)]
    [Header("子弹命中特效")]
    #endregion

    #region Tooltip
    [Tooltip("填入子弹命特效的颜色渐变器")]
    #endregion
    public Gradient colorGradient;

    #region Tooltip
    [Tooltip("持续时间")]
    #endregion
    public float duration = 0.5f;

    #region Tooltip
    [Tooltip("起始粒子大小")]
    #endregion
    public float startParticleSize = 0.25f;

    #region Tooltip
    [Tooltip("起始粒子速度")]
    #endregion
    public float startParticleSpeed = 3f;

    #region Tooltip
    [Tooltip("起始粒子生命线")]
    #endregion
    public float startLifeTime = 0.5f;

    #region Tooltip
    [Tooltip("粒子的最大数量")]
    #endregion
    public int maxdParticlesNumber = 100;

    #region Tooltip
    [Tooltip("粒子的发射速率")]
    #endregion
    public int emissionRate = 100;

    #region Tooltip
    [Tooltip("粒子爆发数量")]
    #endregion
    public int burstParticleNumber = 20;

    #region Tooltip
    [Tooltip("粒子重力参数")]
    #endregion
    public float effectGravity = -0.01f;

    #region Tooltip
    [Tooltip("粒子精灵")]
    #endregion
    public Sprite effectSprite;

    #region Tooltip
    [Tooltip("生命周期内最慢速度")]
    #endregion
    public Vector3 velocityOverLifeTimeMin;

    #region Tooltip
    [Tooltip("生命周期内最快速度")]
    #endregion
    public Vector3 velocityOverLifeTimeMax;

    #region Tooltip
    [Tooltip("子弹命特效预制体")]
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
