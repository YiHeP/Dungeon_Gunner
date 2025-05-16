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
    public CurrentPlayerSO currentPlayer;

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

    #region Tooltip
    [Tooltip("����materialize��ɫ��")]
    #endregion
    public Shader materializeShader;

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

    #region Tooltip
    [Tooltip("�����������ֵͼ��")]
    #endregion
    public GameObject heartIconPrefab;

    #region Tooltip
    [Tooltip("�÷�Ԥ����")]
    #endregion
    public GameObject scorePrefab;

    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion

    #region Tooltip
    [Tooltip("������ƷԤ����")]
    #endregion
    public GameObject chestItemPerfab;

    #region Tooltip
    [Tooltip("��������ֵͼ��")]
    #endregion
    public Sprite heartIcon;

    #region Tooltip
    [Tooltip("�����ӵ�ͼ��")]
    #endregion
    public Sprite bulletIcon;

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

    #region Tooltip
    [Tooltip("�����Ƶ�����Ч")]
    #endregion
    public SoundEffectSO tableFilp;

    #region Tooltip
    [Tooltip("����򿪵���Ч")]
    #endregion
    public SoundEffectSO chestOpen;

    #region Tooltip
    [Tooltip("����ֵʰȡ��Ч")]
    #endregion
    public SoundEffectSO healthPickUp;

    #region Tooltip
    [Tooltip("�ӵ�ʰȡ��Ч")]
    #endregion
    public SoundEffectSO ammoPickUp;

    #region Tooltip
    [Tooltip("����ʰȡ��Ч")]
    #endregion
    public SoundEffectSO weaponPickUp;

    #region Header С��ͼ
    [Space(10)]
    [Header("С��ͼ")]
    #endregion

    #region Tooltip
    [Tooltip("С��ͼ�ϵ�����ͼ��")]
    #endregion
    public GameObject miniMapSkullPrefab;

    #region Header ����
    [Space(10)]
    [Header("����")]
    #endregion

    #region Tooltip
    [Tooltip("�������ֿ�����")]
    #endregion
    public AudioMixerGroup musicMasterMixerGroup;

    #region Tooltip
    [Tooltip("����������������")]
    #endregion
    public AudioMixerSnapshot musicOnFullSnapshot;

    #region Tooltip
    [Tooltip("�������ֵͿ���")]
    #endregion
    public AudioMixerSnapshot musicLoweSnapshot;

    #region Tooltip
    [Tooltip("�������ֹرտ���")]
    #endregion
    public AudioMixerSnapshot musicOffSnapshot;

    #region Tooltip
    [Tooltip("��������������")]
    #endregion
    public MusicTrackSO mainMenuMusic;

    #region Header ���ѡ��
    [Space(10)]
    [Header("���ѡ��")]
    #endregion

    #region Tooltip
    [Tooltip("���ѡ���Ԥ����")]
    #endregion
    public GameObject playerSelectionPrefab;

    #region Tooltip
    [Tooltip("�������ϸ���б�")]
    #endregion
    public List<PlayerDetailsSO> playerDetailsList;

    #region Validation
#if UNITY_EDITOR
    public void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(roomNodeTypelist), roomNodeTypelist);
        HelpUtilities.ValidateCheckNullValues(this, nameof(currentPlayer), currentPlayer);
        HelpUtilities.ValidateCheckNullValues(this, nameof(dimmedMaterial), dimmedMaterial);
        HelpUtilities.ValidateCheckNullValues(this, nameof(variableLitShader), variableLitShader);
        HelpUtilities.ValidateCheckNullValues(this, nameof(ammoIconPrefab), ammoIconPrefab);
        HelpUtilities.ValidateCheckNullValues(this, nameof(soundMasterMixrGroup), soundMasterMixrGroup);
        HelpUtilities.ValidateCheckNullValues(this, nameof(doorOpenCloseSoundEffect), doorOpenCloseSoundEffect);
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(enemyUnwalkableCollisionTilesArray), enemyUnwalkableCollisionTilesArray);
        HelpUtilities.ValidateCheckNullValues(this, nameof(perferredEnemyPathTile), perferredEnemyPathTile);
        HelpUtilities.ValidateCheckNullValues(this,nameof(tableFilp),tableFilp);
        HelpUtilities.ValidateCheckNullValues(this,nameof(heartIconPrefab), heartIconPrefab);
        HelpUtilities.ValidateCheckNullValues(this, nameof(chestOpen), chestOpen);
        HelpUtilities.ValidateCheckNullValues(this, nameof(healthPickUp), healthPickUp);
        HelpUtilities.ValidateCheckNullValues(this, nameof(ammoPickUp), ammoPickUp);
        HelpUtilities.ValidateCheckNullValues(this, nameof(weaponPickUp), weaponPickUp);
        HelpUtilities.ValidateCheckNullValues(this, nameof(materializeShader), materializeShader);
        HelpUtilities.ValidateCheckNullValues(this, nameof(chestItemPerfab), chestItemPerfab);
        HelpUtilities.ValidateCheckNullValues(this, nameof(heartIcon), heartIcon);
        HelpUtilities.ValidateCheckNullValues(this, nameof(bulletIcon), bulletIcon);
        HelpUtilities.ValidateCheckNullValues(this, nameof(miniMapSkullPrefab), miniMapSkullPrefab);
        HelpUtilities.ValidateCheckNullValues(this, nameof(musicMasterMixerGroup), musicMasterMixerGroup);
        HelpUtilities.ValidateCheckNullValues(this, nameof(musicOnFullSnapshot), musicOnFullSnapshot);
        HelpUtilities.ValidateCheckNullValues(this, nameof(musicLoweSnapshot), musicLoweSnapshot);
        HelpUtilities.ValidateCheckNullValues(this, nameof(musicOffSnapshot), musicOffSnapshot);
        HelpUtilities.ValidateCheckNullValues(this, nameof(mainMenuMusic), mainMenuMusic);
        HelpUtilities.ValidateCheckNullValues(this, nameof(playerSelectionPrefab), playerSelectionPrefab);
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(playerDetailsList), playerDetailsList);
        HelpUtilities.ValidateCheckNullValues(this, nameof(scorePrefab), scorePrefab);
    }
#endif
    #endregion

}
