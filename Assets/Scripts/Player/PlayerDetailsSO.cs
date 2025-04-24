using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/玩家细节")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header 玩家基本细节
    [Space(10)]
    [Header("玩家基本细节")]
    #endregion
    #region Tooltip
    [Tooltip("玩家角色姓名")]
    #endregion
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("玩家预制体")]
    #endregion
    public GameObject playerPrefab;

    #region Tooltip
    [Tooltip("运行时玩家的动画控制器")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header 生命值
    [Space(10)]
    [Header("生命值")]
    #endregion
    #region Tooltip
    [Tooltip("玩家起始生命值")]
    #endregion
    public int PlayerHealthAmount;

    #region Header 武器
    [Space(10)]
    [Header("武器")]
    #endregion

    #region Tooltip
    [Tooltip("玩家的初始武器")]
    #endregion
    public WeaponsDetailsSO staringWeapon;

    #region Tooltip
    [Tooltip("填入玩家的初始武器列表")]
    #endregion
    public List<WeaponsDetailsSO> stasrtingWeaponList;

    #region Header 其余
    [Space(10)]
    [Header("其余")]
    #endregion
    #region Tooltip
    [Tooltip("用于小地图上的玩家图标精灵")]
    #endregion
    public Sprite PlayerMiniMapIcon;

    #region Tooltip
    [Tooltip("玩家手部精灵")]
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
