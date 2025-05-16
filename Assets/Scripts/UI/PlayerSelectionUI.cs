using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSelectionUI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("填入玩家手的精灵")]
    #endregion
    public SpriteRenderer playerHandSpriteRenderer;

    #region Tooltip
    [Tooltip("填入玩家没有武器的手的精灵")]
    #endregion
    public SpriteRenderer playerHandNoWeaponSpriteRenderer;

    #region Tooltip
    [Tooltip("填入玩家武器的精灵")]
    #endregion
    public SpriteRenderer playerWeaponSpriteRenderer;

    #region Tooltip
    [Tooltip("填入玩家动画")]
    #endregion
    public Animator animator;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(playerHandSpriteRenderer), playerHandSpriteRenderer);
        HelpUtilities.ValidateCheckNullValues(this, nameof(playerHandNoWeaponSpriteRenderer), playerHandNoWeaponSpriteRenderer);
        HelpUtilities.ValidateCheckNullValues(this, nameof(playerWeaponSpriteRenderer), playerWeaponSpriteRenderer);
        HelpUtilities.ValidateCheckNullValues(this, nameof(animator), animator);
    }
#endif
    #endregion
}
