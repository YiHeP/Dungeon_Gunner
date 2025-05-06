using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/����ϸ��")]
public class WeaponsDetailsSO : ScriptableObject
{
    #region ��������ϸ��
    [Space(10)]
    [Header("�����Ļ���ϸ��")]
    #endregion
    #region Tooltip
    [Tooltip("��������")]
    #endregion
    public string weaponName;

    #region Tooltip
    [Tooltip("�������õľ���-��Ҫ��������������״")]
    #endregion
    public Sprite weaponSprite;

    #region ����������
    [Space(10)]
    [Header("��������")]
    #endregion
    #region Tooltip
    [Tooltip("���������λ��")]
    #endregion
    public Vector3 weaponShootPosition;
    #region Tooltip
    [Tooltip("�����������õ�ҩ")]
    #endregion
    public AmmoDetailSO weaponCurrentAmmo;

    #region �����Ĳ���ֵ
    [Space(10)]
    [Header("�����Ĳ���ֵ")]
    #endregion
    #region Tooltip
    [Tooltip("���������Ƿ�Ϊ���޵�ҩ")]
    #endregion
    public bool hasInfiniteAmmo = false;

    #region Tooltip
    [Tooltip("���������Ƿ�Ϊ���޵���")]
    #endregion
    public bool hasInfiniteClipCapacity = false;

    #region Tooltip
    [Tooltip("�����ĵ�������")]
    #endregion
    public int weaponClipAmmoCapacity = 6;

    #region Tooltip
    [Tooltip("�������ܵ�ҩ��")]
    #endregion
    public int weaponAmmoCapacity = 100;

    #region Tooltip
    [Tooltip("���������ٶ�-�����뷢��һ��")]
    #endregion
    public float weaponFireRate = 0.2f;

    #region Tooltip
    [Tooltip("����Ԥ��ʱ��")]
    #endregion
    public float weaponPrechargeTime = 0f;

    #region Tooltip
    [Tooltip("������װ��ʱ��")]
    #endregion
    public float weaponReloadTime = 0f;

    #region Tooltip
    [Tooltip("���������Ч")]
    #endregion
    public SoundEffectSO weaponFiringSoundEffect;

    #region Tooltip
    [Tooltip("����װ����Ч")]
    #endregion
    public SoundEffectSO wepaonReloadingSoundEffect;

    #region Tooltip
    [Tooltip("���������Ч")]
    #endregion
    public WeaponShootEffectSO weaponShootEffect;
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(weaponFireRate), weaponFireRate, false);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if(!hasInfiniteAmmo)
        {
            HelpUtilities.ValidateCheckPositiveValues(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }
        if(!hasInfiniteClipCapacity)
        {
            HelpUtilities.ValidateCheckPositiveValues(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }
#endif
    #endregion
}
