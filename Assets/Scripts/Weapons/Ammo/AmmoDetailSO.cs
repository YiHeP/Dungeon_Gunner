using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapon/子弹细节")]
public class AmmoDetailSO : ScriptableObject
{
    #region 子弹基本信息
    [Space(10)]
    [Header("子弹基本信息")]
    #endregion

    #region Tooltip
    [Tooltip("子弹名称")]
    #endregion
    public string ammoName;

    public bool isPlayerAmmo;

    #region Tooltip
    [Tooltip("填入子弹命中特效")]
    #endregion
    public AmmoHitEffectSO ammoHitEffect;

    #region 子弹的预制体，精灵，材质
    [Space(10)]
    [Header("子弹的预制体，精灵，材质")]
    #endregion

    #region Tooltip
    [Tooltip("子弹用的精灵")]
    #endregion
    public Sprite ammoSprite;

    #region Tooltip
    [Tooltip("如果有多个子弹预制体，可以填入其中进行使用")]
    #endregion
    public GameObject[] ammoPrefabArray;

    #region Tooltip
    [Tooltip("子弹用的材质")]
    #endregion
    public Material ammoMaterial;

    #region Tooltip
    [Tooltip("释放之前的停止时间")]
    #endregion
    public float ammoChargeTime = 0.1f;

    #region
    [Tooltip("释放的材质")]
    #endregion
    public Material ammoChargeMaterial;

    #region Header 子弹基本参数
    [Space(10)]
    [Header("子弹基本参数")]
    #endregion

    #region Tooltip
    [Tooltip("子弹伤害")]
    #endregion
    public int ammoDamage = 1;

    #region Tooltip
    [Tooltip("子弹的最低速度，子弹速度将在随机在最低和最高之间")]
    #endregion
    public float ammoSpeedMin = 20f;

    #region Tooltip
    [Tooltip("子弹的最高速度，子弹速度将在随机在最低和最高之间")]
    #endregion
    public float ammoSpeedMax = 20f;

    #region Tooltip
    [Tooltip("子弹或子弹模式在unity单位中的范围")]
    #endregion
    public float ammoRange = 20f;

    #region Tooltip
    [Tooltip("子弹每秒旋转多少度")]
    #endregion
    public float ammoRotationSpeed = 1f;

    #region Header 子弹散布细节
    [Space(10)]
    [Header("子弹散布细节")]
    #endregion

    #region Tooltip
    [Tooltip("子弹散布的最小值")]
    #endregion
    public float ammoSpreadMin = 0f;

    #region Tooltip
    [Tooltip("子弹散布的最大值")]
    #endregion
    public float ammoSpreadMax = 0f;

    #region Header 子弹生成细节
    [Space(10)]
    [Header("子弹生成细节")]
    #endregion

    #region Tooltip
    [Tooltip("弹药生成量的最小值")]
    #endregion
    public int ammoSpawnAmoutMin = 1;

    #region Tooltip
    [Tooltip("弹药生成量的最大值")]
    #endregion
    public int ammoSpawnAmoutMax = 1;

    #region Tooltip
    [Tooltip("子弹生成时间间隔最短时间")]
    #endregion
    public float ammoSpawnIntervalMin = 0f;

    #region Tooltip
    [Tooltip("子弹生成时间间隔最长时间")]
    #endregion
    public float ammoSpawnIntervalMax = 0f;

    #region Header 子弹痕迹细节
    [Space(10)]
    [Header("子弹痕迹细节")]
    #endregion

    #region Tooltip
    [Tooltip("子弹是否需要痕迹")]
    #endregion
    public bool isAmmoTrail = false;

    #region Tooltip
    [Tooltip("子弹痕迹留存时间")]
    #endregion
    public float ammoTrailTime = 3f;

    #region Tooltip
    [Tooltip("子弹痕迹材料")]
    #endregion
    public Material ammoTrailMaterial;

    #region Tooltip
    [Tooltip("子弹痕迹起始宽度")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailStartWidth;

    #region Tooltip
    [Tooltip("子弹痕迹结束宽度")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailEndtWidth;


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
        HelpUtilities.ValidateCheckNullValues(this,nameof(ammoSprite),ammoSprite);
        HelpUtilities.ValidateCheckEnumerableValues(this,nameof(ammoPrefabArray),ammoPrefabArray);
        HelpUtilities.ValidateCheckNullValues(this,nameof(ammoMaterial),ammoMaterial);
        if(ammoChargeTime > 0f)
            HelpUtilities.ValidateCheckNullValues(this,nameof(ammoChargeMaterial),ammoChargeMaterial);
        HelpUtilities.ValidateCheckPositiveValues(this,nameof(ammoDamage),ammoDamage,false);
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(ammoSpeedMin), ammoSpeedMin, nameof(ammoSpeedMax), ammoSpeedMax, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(ammoRange), ammoRange, false);
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(ammoSpreadMin),ammoSpreadMin,nameof(ammoSpreadMax),ammoSpreadMax,true);
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(ammoSpawnAmoutMin),ammoSpawnAmoutMin,nameof(ammoSpawnAmoutMax),ammoSpawnAmoutMax,false);
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(ammoSpawnIntervalMin),ammoSpawnIntervalMin,nameof(ammoSpawnIntervalMax),
            ammoSpawnIntervalMax,true);
        if(isAmmoTrail)
        {
            HelpUtilities.ValidateCheckPositiveValues(this,nameof(ammoTrailTime),ammoTrailTime,false);
            HelpUtilities.ValidateCheckNullValues(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelpUtilities.ValidateCheckPositiveValues(this,nameof(ammoTrailStartWidth),ammoTrailStartWidth,false);
            HelpUtilities.ValidateCheckPositiveValues(this,nameof(ammoTrailEndtWidth),ammoTrailEndtWidth,false);
        }

    }
#endif
    #endregion

}
