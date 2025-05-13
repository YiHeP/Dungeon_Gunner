using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Environment : MonoBehaviour
{
    #region Header 引用
    [Space(10)]
    [Header("引用")]
    #endregion

    #region Tooltip
    [Tooltip("填入精灵渲染器")]
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
