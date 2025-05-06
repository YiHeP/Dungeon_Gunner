using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapon/�ӵ�ϸ��")]
public class AmmoDetailSO : ScriptableObject
{
    #region �ӵ�������Ϣ
    [Space(10)]
    [Header("�ӵ�������Ϣ")]
    #endregion

    #region Tooltip
    [Tooltip("�ӵ�����")]
    #endregion
    public string ammoName;

    public bool isPlayerAmmo;

    #region Tooltip
    [Tooltip("�����ӵ�������Ч")]
    #endregion
    public AmmoHitEffectSO ammoHitEffect;

    #region �ӵ���Ԥ���壬���飬����
    [Space(10)]
    [Header("�ӵ���Ԥ���壬���飬����")]
    #endregion

    #region Tooltip
    [Tooltip("�ӵ��õľ���")]
    #endregion
    public Sprite ammoSprite;

    #region Tooltip
    [Tooltip("����ж���ӵ�Ԥ���壬�����������н���ʹ��")]
    #endregion
    public GameObject[] ammoPrefabArray;

    #region Tooltip
    [Tooltip("�ӵ��õĲ���")]
    #endregion
    public Material ammoMaterial;

    #region Tooltip
    [Tooltip("�ͷ�֮ǰ��ֹͣʱ��")]
    #endregion
    public float ammoChargeTime = 0.1f;

    #region
    [Tooltip("�ͷŵĲ���")]
    #endregion
    public Material ammoChargeMaterial;

    #region Header �ӵ���������
    [Space(10)]
    [Header("�ӵ���������")]
    #endregion

    #region Tooltip
    [Tooltip("�ӵ��˺�")]
    #endregion
    public int ammoDamage = 1;

    #region Tooltip
    [Tooltip("�ӵ�������ٶȣ��ӵ��ٶȽ����������ͺ����֮��")]
    #endregion
    public float ammoSpeedMin = 20f;

    #region Tooltip
    [Tooltip("�ӵ�������ٶȣ��ӵ��ٶȽ����������ͺ����֮��")]
    #endregion
    public float ammoSpeedMax = 20f;

    #region Tooltip
    [Tooltip("�ӵ����ӵ�ģʽ��unity��λ�еķ�Χ")]
    #endregion
    public float ammoRange = 20f;

    #region Tooltip
    [Tooltip("�ӵ�ÿ����ת���ٶ�")]
    #endregion
    public float ammoRotationSpeed = 1f;

    #region Header �ӵ�ɢ��ϸ��
    [Space(10)]
    [Header("�ӵ�ɢ��ϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("�ӵ�ɢ������Сֵ")]
    #endregion
    public float ammoSpreadMin = 0f;

    #region Tooltip
    [Tooltip("�ӵ�ɢ�������ֵ")]
    #endregion
    public float ammoSpreadMax = 0f;

    #region Header �ӵ�����ϸ��
    [Space(10)]
    [Header("�ӵ�����ϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("��ҩ����������Сֵ")]
    #endregion
    public int ammoSpawnAmoutMin = 1;

    #region Tooltip
    [Tooltip("��ҩ�����������ֵ")]
    #endregion
    public int ammoSpawnAmoutMax = 1;

    #region Tooltip
    [Tooltip("�ӵ�����ʱ�������ʱ��")]
    #endregion
    public float ammoSpawnIntervalMin = 0f;

    #region Tooltip
    [Tooltip("�ӵ�����ʱ�����ʱ��")]
    #endregion
    public float ammoSpawnIntervalMax = 0f;

    #region Header �ӵ��ۼ�ϸ��
    [Space(10)]
    [Header("�ӵ��ۼ�ϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("�ӵ��Ƿ���Ҫ�ۼ�")]
    #endregion
    public bool isAmmoTrail = false;

    #region Tooltip
    [Tooltip("�ӵ��ۼ�����ʱ��")]
    #endregion
    public float ammoTrailTime = 3f;

    #region Tooltip
    [Tooltip("�ӵ��ۼ�����")]
    #endregion
    public Material ammoTrailMaterial;

    #region Tooltip
    [Tooltip("�ӵ��ۼ���ʼ���")]
    #endregion
    [Range(0f, 1f)] public float ammoTrailStartWidth;

    #region Tooltip
    [Tooltip("�ӵ��ۼ��������")]
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
