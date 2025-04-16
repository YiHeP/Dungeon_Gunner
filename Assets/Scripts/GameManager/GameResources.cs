using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    #region Header ����
    [Space(10)]
    [Header("���η�������")]
    #endregion
    #region Tooltip
    [Tooltip("��Ϸ�����еķ�������")]
    #endregion
    public RoomNodeTypeListSO roomNodeTypelist;

    #region Header ���
    [Space(10)]
    [Header("���")]
    #endregion
    #region Tooltip
    [Tooltip("��ǰ��ҵĿɱ�̽ű�����")]
    #endregion
    public CurrentPlayerSO currentPlayerSO;

    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion
    #region Tooltip
    [Tooltip("��������")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("�������Ĭ�ϲ���")]
    #endregion
    public Material litMaterial;

    #region Tooltip
    [Tooltip("ʹ�� Variable lit ��ɫ�����")]
    #endregion
    public Shader variableLitShader;

    #region Validation
#if UNITY_EDITOR
    public void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(roomNodeTypelist), roomNodeTypelist);
        HelpUtilities.ValidateCheckNullValues(this, nameof(currentPlayerSO), currentPlayerSO);
        HelpUtilities.ValidateCheckNullValues(this, nameof(dimmedMaterial), dimmedMaterial);
        HelpUtilities.ValidateCheckNullValues(this, nameof(variableLitShader), variableLitShader);
    }
#endif
    #endregion

}
