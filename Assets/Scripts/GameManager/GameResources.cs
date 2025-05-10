using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Audio;

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

    #region Header ������Ƭ��ͼ����Ƭ
    [Space(10)]
    [Header("������Ƭ��ͼ����Ƭ")]
    #endregion
    #region Tooltip
    [Tooltip("���˿��Ե���������ײͼ��")]
    #endregion
    public TileBase[] enemyUnwalkableCollisionTilesArray;
    #region Tooltip
    [Tooltip("�з���������ѡ·��ͼ��")]
    #endregion
    public TileBase perferredEnemyPathTile;

    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion
    #region Tooltip
    [Tooltip("�����ӵ�ͼ��")]
    #endregion
    public GameObject ammoIconPrefab;

    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion
    #region Tooltip
    [Tooltip("���������Ͽ�������")]
    #endregion
    public AudioMixerGroup soundMasterMixrGroup;
    #region Tooltip
    [Tooltip("�ſ��ص���Ч")]
    #endregion
    public SoundEffectSO doorOpenCloseSoundEffect;

    #region Validation
#if UNITY_EDITOR
    public void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(roomNodeTypelist), roomNodeTypelist);
        HelpUtilities.ValidateCheckNullValues(this, nameof(currentPlayerSO), currentPlayerSO);
        HelpUtilities.ValidateCheckNullValues(this, nameof(dimmedMaterial), dimmedMaterial);
        HelpUtilities.ValidateCheckNullValues(this, nameof(variableLitShader), variableLitShader);
        HelpUtilities.ValidateCheckNullValues(this, nameof(ammoIconPrefab), ammoIconPrefab);
        HelpUtilities.ValidateCheckNullValues(this, nameof(soundMasterMixrGroup), soundMasterMixrGroup);
        HelpUtilities.ValidateCheckNullValues(this, nameof(doorOpenCloseSoundEffect), doorOpenCloseSoundEffect);
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(enemyUnwalkableCollisionTilesArray), enemyUnwalkableCollisionTilesArray);
        HelpUtilities.ValidateCheckNullValues(this, nameof(perferredEnemyPathTile), perferredEnemyPathTile);
    }
#endif
    #endregion

}
