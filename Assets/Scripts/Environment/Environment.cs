using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Environment : MonoBehaviour
{
    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion

    #region Tooltip
    [Tooltip("���뾫����Ⱦ��")]
    #endregion
    public SpriteRenderer spriteRenderer;

    #region Valiadation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this,nameof(spriteRenderer),spriteRenderer);   
    }
#endif
    #endregion
}
