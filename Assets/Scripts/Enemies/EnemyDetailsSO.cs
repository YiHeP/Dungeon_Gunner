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

    #region Header ���˲���
    [Space(10)]
    [Header("���˲���")]
    #endregion

    #region Tooltip
    [Tooltip("����վ������")]
    #endregion
    public Material enemyStandarMaterial;

    #region Header ���˲���ϸ��
    [Space(10)]
    [Header("���˲���ϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("�������ɲ��ʳ���ʱ��")]
    #endregion
    public float enemyMaterializeTime;

    #region Tooltip
    [Tooltip("�������ɲ��ʵ���ɫ��")]
    #endregion
    public Shader enemyMaterializeShader;

    [ColorUsage(true, true)]
    #region Tooltip
    [Tooltip("������ɫ")]
    #endregion
    public Color enemyMaterializeColor;

    #region Header ��������ϸ��
    [Space(10)]
    [Header("��������ϸ��")]
    #endregion

    #region Tooltip
    [Tooltip("���˵�����")]
    #endregion
    public WeaponsDetailsSO enemyWepapon;

    #region Tooltip
    [Tooltip("����������С���ʱ��")]
    #endregion
    public float firingIntervalMin = 0.1f;

    #region Tooltip
    [Tooltip("������������ʱ��")]
    #endregion
    public float firingIntervalMax = 1f;

    #region Tooltip
    [Tooltip("���˷�����̳���ʱ��")]
    #endregion
    public float firingDurationMin = 1f;

    #region Tooltip
    [Tooltip("���˷��������ʱ��")]
    #endregion
    public float firingDurationMax = 2f;

    #region Tooltip
    [Tooltip("�Ƿ���Ҫ��ҵ���Ұ��Ϣ���п���")]
    #endregion
    public bool firingLineOfSightRequired;

    #region Header ��������ֵ
    [Space(10)]
    [Header("��������ֵ")]
    #endregion

    #region Tooltip
    [Tooltip("�����ڵ���ÿ���ؿ�������ֵ")]
    #endregion
    public EnemyHealthDetail[] enemyHealthDetailsArray;

    #region Tooltip
    [Tooltip("�ڱ����к��Ƿ������һ��ʱ��")]
    #endregion
    public bool isImmuneAfterHit = false;

    #region Tooltip
    [Tooltip("���ߵ�ʱ��")]
    #endregion
    public float hitTmunityTime;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(enemyName),enemyName);
        HelpUtilities.ValidateCheckNullValues(this,nameof(enemyPrefab),enemyPrefab);
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(chaseDistance), chaseDistance, false);
        HelpUtilities.ValidateCheckNullValues(this, nameof(enemyStandarMaterial), enemyStandarMaterial);
        HelpUtilities.ValidateCheckPositiveValues(this,nameof(enemyMaterializeTime), enemyMaterializeTime, true);
        HelpUtilities.ValidateCheckNullValues(this, nameof(enemyMaterializeShader),enemyMaterializeShader);
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(firingIntervalMin), firingIntervalMin, nameof(firingIntervalMax),
            firingIntervalMax, false);
        HelpUtilities.ValidateCheckPositiveRange(this, nameof(firingDurationMin), firingDurationMin, nameof(firingDurationMax),
            firingDurationMax, false);
        HelpUtilities.ValidateCheckEnumerableValues(this,nameof(enemyHealthDetailsArray),enemyHealthDetailsArray);
        if(isImmuneAfterHit)
        {
            HelpUtilities.ValidateCheckPositiveValues(this,nameof(hitTmunityTime), hitTmunityTime, false);
        }
    }
#endif
    #endregion

}
