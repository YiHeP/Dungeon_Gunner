using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/Movement/�ƶ�ϸ��")]
public class MovementDetailsSO : ScriptableObject
{
    #region Header �ƶ�ϸ��
    [Space(10)]
    [Header("�ƶ�ϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("����ƶ��ٶ�")]
    #endregion
    public float minMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("����ƶ��ٶ�")]
    #endregion
    public float maxMoveSpeed = 8f;

    #region Tooltip
    [Tooltip("��ҵķ����ٶ�")]
    #endregion
    public float rollSpeed = 0f;

    #region Tooltip
    [Tooltip("��ҵķ�������")]
    #endregion
    public float rollDistance = 0f;

    #region Tooltip
    [Tooltip("��ҵķ�����ȴ")]
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
