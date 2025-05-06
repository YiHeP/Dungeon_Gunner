using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/武器细节")]
public class WeaponsDetailsSO : ScriptableObject
{
    #region 武器基本细节
    [Space(10)]
    [Header("武器的基本细节")]
    #endregion
    #region Tooltip
    [Tooltip("武器名称")]
    #endregion
    public string weaponName;

    #region Tooltip
    [Tooltip("武器所用的精灵-需要开启生成物理形状")]
    #endregion
    public Sprite weaponSprite;

    #region 武器的配置
    [Space(10)]
    [Header("武器配置")]
    #endregion
    #region Tooltip
    [Tooltip("武器的射击位置")]
    #endregion
    public Vector3 weaponShootPosition;
    #region Tooltip
    [Tooltip("武器现在所用弹药")]
    #endregion
    public AmmoDetailSO weaponCurrentAmmo;

    #region 武器的操作值
    [Space(10)]
    [Header("武器的操作值")]
    #endregion
    #region Tooltip
    [Tooltip("决定武器是否为无限弹药")]
    #endregion
    public bool hasInfiniteAmmo = false;

    #region Tooltip
    [Tooltip("决定武器是否为无限弹夹")]
    #endregion
    public bool hasInfiniteClipCapacity = false;

    #region Tooltip
    [Tooltip("武器的弹夹容量")]
    #endregion
    public int weaponClipAmmoCapacity = 6;

    #region Tooltip
    [Tooltip("武器的总弹药量")]
    #endregion
    public int weaponAmmoCapacity = 100;

    #region Tooltip
    [Tooltip("武器开火速度-代表几秒发射一次")]
    #endregion
    public float weaponFireRate = 0.2f;

    #region Tooltip
    [Tooltip("武器预热时间")]
    #endregion
    public float weaponPrechargeTime = 0f;

    #region Tooltip
    [Tooltip("武器的装弹时间")]
    #endregion
    public float weaponReloadTime = 0f;

    #region Tooltip
    [Tooltip("武器射击音效")]
    #endregion
    public SoundEffectSO weaponFiringSoundEffect;

    #region Tooltip
    [Tooltip("武器装填音效")]
    #endregion
    public SoundEffectSO wepaonReloadingSoundEffect;

    #region Tooltip
    [Tooltip("武器射击特效")]
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
