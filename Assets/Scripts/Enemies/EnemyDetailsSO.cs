using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails_",menuName = "Scriptable Objects/Enemy/����ϸ��")]
public class EnemyDetailsSO : ScriptableObject
{
    #region Header ���˻�����Ϣ
    [Space(10)]
    [Header("���˻�����Ϣ")]
    #endregion

    #region Tooltip
    [Tooltip("��������")]
    #endregion
    public string enemyName;

    #region Tooltip
    [Tooltip("����Ԥ����")]
    #endregion
    public GameObject enemyPrefab;

    #region Tooltip
    [Tooltip("��Ҿ�������Զ��������׷��")]
    #endregion
    public float chaseDistance = 50f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(enemyName),enemyName);
        HelpUtilities.ValidateCheckNullValues(this,nameof(enemyPrefab),enemyPrefab);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(chaseDistance), chaseDistance, false);
    }
#endif
    #endregion

}
