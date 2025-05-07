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

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(enemyName),enemyName);
        HelpUtilities.ValidateCheckNullValues(this,nameof(enemyPrefab),enemyPrefab);
    }
#endif
    #endregion

}
