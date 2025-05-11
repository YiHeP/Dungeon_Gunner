using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_",menuName = "Scriptable Objects/Enemy/敌人细节")]
public class EnemyDetailsSO : ScriptableObject
{
    #region Header 敌人基础信息
    [Space(10)]
    [Header("敌人基础信息")]
    #endregion

    #region Tooltip
    [Tooltip("敌人名称")]
    #endregion
    public string enemyName;

    #region Tooltip
    [Tooltip("敌人预制体")]
    #endregion
    public GameObject enemyPrefab;

    #region Tooltip
    [Tooltip("玩家距离怪物多远怪物启动追逐")]
    #endregion
    public float chaseDistance = 50f;

    #region Header 敌人材质
    [Space(10)]
    [Header("敌人材质")]
    #endregion

    #region Tooltip
    [Tooltip("敌人站立材质")]
    #endregion
    public Material enemyStandarMaterial;

    #region Header 敌人材质细节
    [Space(10)]
    [Header("敌人材质细节")]
    #endregion

    #region Tooltip
    [Tooltip("敌人生成材质持续时间")]
    #endregion
    public float enemyMaterializeTime;

    #region Tooltip
    [Tooltip("敌人生成材质的着色器")]
    #endregion
    public Shader enemyMaterializeShader;

    [ColorUsage(true, true)]
    #region Tooltip
    [Tooltip("材质颜色")]
    #endregion
    public Color enemyMaterializeColor;

    #region Header 敌人武器细节
    [Space(10)]
    [Header("敌人武器细节")]
    #endregion

    #region Tooltip
    [Tooltip("敌人的武器")]
    #endregion
    public WeaponsDetailsSO enemyWepapon;

    #region Tooltip
    [Tooltip("武器发射最小间隔时间")]
    #endregion
    public float firingIntervalMin = 0.1f;

    #region Tooltip
    [Tooltip("武器发射最长间隔时间")]
    #endregion
    public float firingIntervalMax = 1f;

    #region Tooltip
    [Tooltip("敌人发射最短持续时间")]
    #endregion
    public float firingDurationMin = 1f;

    #region Tooltip
    [Tooltip("敌人发射最长持续时间")]
    #endregion
    public float firingDurationMax = 2f;

    #region Tooltip
    [Tooltip("是否需要玩家的视野信息进行开火")]
    #endregion
    public bool firingLineOfSightRequired;

    #region Header 敌人生命值
    [Space(10)]
    [Header("敌人生命值")]
    #endregion

    #region Tooltip
    [Tooltip("敌人在地牢每个关卡的生命值")]
    #endregion
    public EnemyHealthDetail[] enemyHealthDetailsArray;

    #region Tooltip
    [Tooltip("在被击中后是否会免疫一段时间")]
    #endregion
    public bool isImmuneAfterHit = false;

    #region Tooltip
    [Tooltip("免疫的时长")]
    #endregion
    public float hitTmunityTime;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(enemyName),enemyName);
        HelpUtilities.ValidateCheckNullValues(this,nameof(enemyPrefab),enemyPrefab);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(chaseDistance), chaseDistance, false);
        HelpUtilities.ValidateCheckNullValues(this, nameof(enemyStandarMaterial), enemyStandarMaterial);
        HelpUtilities.ValidateCheckPositiveValues(this,nameof(enemyMaterializeTime), enemyMaterializeTime, true);
        HelpUtilities.ValidateCheckNullValues(this, nameof(enemyMaterializeShader),enemyMaterializeShader);
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin, nameof(firingIntervalMax),
            firingIntervalMax, false);
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax),
            firingDurationMax, false);
        HelpUtilities.ValidateCheckEnumerableValues(this,nameof(enemyHealthDetailsArray),enemyHealthDetailsArray);
        if(isImmuneAfterHit)
        {
            HelpUtilities.ValidateCheckPositiveValues(this,nameof(hitTmunityTime), hitTmunityTime, false);
        }
    }
#endif
    #endregion

}
