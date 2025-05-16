using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerSelectionUI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("��������ֵľ���")]
    #endregion
    public SpriteRenderer playerHandSpriteRenderer;

    #region Tooltip
    [Tooltip("�������û���������ֵľ���")]
    #endregion
    public SpriteRenderer playerHandNoWeaponSpriteRenderer;

    #region Tooltip
    [Tooltip("������������ľ���")]
    #endregion
    public SpriteRenderer playerWeaponSpriteRenderer;

    #region Tooltip
    [Tooltip("������Ҷ���")]
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
