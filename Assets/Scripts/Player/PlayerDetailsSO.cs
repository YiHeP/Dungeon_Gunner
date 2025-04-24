using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/���ϸ��")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header ��һ���ϸ��
    [Space(10)]
    [Header("��һ���ϸ��")]
    #endregion
    #region Tooltip
    [Tooltip("��ҽ�ɫ����")]
    #endregion
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("���Ԥ����")]
    #endregion
    public GameObject playerPrefab;

    #region Tooltip
    [Tooltip("����ʱ��ҵĶ���������")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header ����ֵ
    [Space(10)]
    [Header("����ֵ")]
    #endregion
    #region Tooltip
    [Tooltip("�����ʼ����ֵ")]
    #endregion
    public int PlayerHealthAmount;

    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion

    #region Tooltip
    [Tooltip("��ҵĳ�ʼ����")]
    #endregion
    public WeaponsDetailsSO staringWeapon;

    #region Tooltip
    [Tooltip("������ҵĳ�ʼ�����б�")]
    #endregion
    public List<WeaponsDetailsSO> stasrtingWeaponList;

    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion
    #region Tooltip
    [Tooltip("����С��ͼ�ϵ����ͼ�꾫��")]
    #endregion
    public Sprite PlayerMiniMapIcon;

    #region Tooltip
    [Tooltip("����ֲ�����")]
    #endregion
    public Sprite PlayerHandSprite;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelpUtilities.ValidateCheckNullValues(this,nameof(playerPrefab), playerPrefab);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(PlayerHealthAmount), PlayerHealthAmount, false);
        HelpUtilities.ValidateCheckNullValues(this, nameof(PlayerMiniMapIcon), PlayerMiniMapIcon);
        HelpUtilities.ValidateCheckNullValues(this, nameof(PlayerHandSprite), PlayerHandSprite);
        HelpUtilities.ValidateCheckNullValues(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelpUtilities.ValidateCheckNullValues(this, nameof(staringWeapon),staringWeapon);
        HelpUtilities.ValidateCheckEnumerableValues(this,nameof(stasrtingWeaponList), stasrtingWeaponList);
    }
#endif
    #endregion

}
