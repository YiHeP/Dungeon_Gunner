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

    #region Header 地牢
    [Space(10)]
    [Header("地牢房间类型")]
    #endregion
    #region Tooltip
    [Tooltip("游戏中所有的房间类型")]
    #endregion
    public RoomNodeTypeListSO roomNodeTypelist;

    #region Header 玩家
    [Space(10)]
    [Header("玩家")]
    #endregion
    #region Tooltip
    [Tooltip("当前玩家的可编程脚本物体")]
    #endregion
    public CurrentPlayerSO currentPlayerSO;

    #region Header 材料
    [Space(10)]
    [Header("材料")]
    #endregion
    #region Tooltip
    [Tooltip("暗淡材质")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("精灵光照默认材质")]
    #endregion
    public Material litMaterial;

    #region Tooltip
    [Tooltip("使用 Variable lit 着色器填充")]
    #endregion
    public Shader variableLitShader;

    #region Header 特殊瓦片地图的瓦片
    [Space(10)]
    [Header("特殊瓦片地图的瓦片")]
    #endregion
    #region Tooltip
    [Tooltip("敌人可以导航到的碰撞图块")]
    #endregion
    public TileBase[] enemyUnwalkableCollisionTilesArray;
    #region Tooltip
    [Tooltip("敌方导航的首选路径图块")]
    #endregion
    public TileBase perferredEnemyPathTile;

    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion
    #region Tooltip
    [Tooltip("填入子弹图标")]
    #endregion
    public GameObject ammoIconPrefab;

    #region Header 声音
    [Space(10)]
    [Header("声音")]
    #endregion
    #region Tooltip
    [Tooltip("填充声音混合控制器组")]
    #endregion
    public AudioMixerGroup soundMasterMixrGroup;
    #region Tooltip
    [Tooltip("门开关的音效")]
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
