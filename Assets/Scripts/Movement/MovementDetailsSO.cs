using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/Movement/移动细节")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header 移动细节
    [Space(10)]
    [Header("移动细节")]
    #endregion

    #region Tooltip
    [Tooltip("最低移动速度")]
    #endregion
    public float minMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("最快移动速度")]
    #endregion
    public float maxMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("玩家的翻滚速度")]
    #endregion
    public float rollSpeed = 0f;

    #region Tooltip
    [Tooltip("玩家的翻滚距离")]
    #endregion
    public float rollDistance = 0f;

    #region Tooltip
    [Tooltip("玩家的翻滚冷却")]
    #endregion
    public float rollCoolDownTime = 0f;

    public float GetMoveSpeed()
    {
        if(minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckPositiveRange(this,nameof(minMoveSpeed),minMoveSpeed,nameof(maxMoveSpeed),maxMoveSpeed,false);
        if(rollSpeed != 0 && rollDistance != 0 && rollCoolDownTime != 0)
        {
            HelpUtilities.ValidateCheckPositiveValues(this, nameof(rollSpeed), rollSpeed,false);
            HelpUtilities.ValidateCheckPositiveValues(this, nameof(rollDistance), rollDistance, false);
            HelpUtilities.ValidateCheckPositiveValues(this, nameof(rollCoolDownTime), rollCoolDownTime, false);
        }
    }
#endif
    #endregion

}
