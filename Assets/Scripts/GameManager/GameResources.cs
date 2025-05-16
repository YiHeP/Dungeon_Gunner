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
    public CurrentPlayerSO currentPlayer;

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

    #region Tooltip
    [Tooltip("填入materialize着色器")]
    #endregion
    public Shader materializeShader;

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

    #region Tooltip
    [Tooltip("填入玩家生命值图标")]
    #endregion
    public GameObject heartIconPrefab;

    #region Tooltip
    [Tooltip("得分预制体")]
    #endregion
    public GameObject scorePrefab;

    #region Header 宝箱
    [Space(10)]
    [Header("宝箱")]
    #endregion

    #region Tooltip
    [Tooltip("宝箱物品预制体")]
    #endregion
    public GameObject chestItemPerfab;

    #region Tooltip
    [Tooltip("填入生命值图标")]
    #endregion
    public Sprite heartIcon;

    #region Tooltip
    [Tooltip("填入子弹图标")]
    #endregion
    public Sprite bulletIcon;

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

    #region Tooltip
    [Tooltip("桌子推到的音效")]
    #endregion
    public SoundEffectSO tableFilp;

    #region Tooltip
    [Tooltip("宝箱打开的音效")]
    #endregion
    public SoundEffectSO chestOpen;

    #region Tooltip
    [Tooltip("生命值拾取音效")]
    #endregion
    public SoundEffectSO healthPickUp;

    #region Tooltip
    [Tooltip("子弹拾取音效")]
    #endregion
    public SoundEffectSO ammoPickUp;

    #region Tooltip
    [Tooltip("武器拾取音效")]
    #endregion
    public SoundEffectSO weaponPickUp;

    #region Header 小地图
    [Space(10)]
    [Header("小地图")]
    #endregion

    #region Tooltip
    [Tooltip("小地图上的骷髅图标")]
    #endregion
    public GameObject miniMapSkullPrefab;

    #region Header 音乐
    [Space(10)]
    [Header("音乐")]
    #endregion

    #region Tooltip
    [Tooltip("填入音乐控制组")]
    #endregion
    public AudioMixerGroup musicMasterMixerGroup;

    #region Tooltip
    [Tooltip("填入音乐完整快照")]
    #endregion
    public AudioMixerSnapshot musicOnFullSnapshot;

    #region Tooltip
    [Tooltip("填入音乐低快照")]
    #endregion
    public AudioMixerSnapshot musicLoweSnapshot;

    #region Tooltip
    [Tooltip("填入音乐关闭快照")]
    #endregion
    public AudioMixerSnapshot musicOffSnapshot;

    #region Tooltip
    [Tooltip("填入主界面音乐")]
    #endregion
    public MusicTrackSO mainMenuMusic;

    #region Header 玩家选择
    [Space(10)]
    [Header("玩家选择")]
    #endregion

    #region Tooltip
    [Tooltip("玩家选择的预制体")]
    #endregion
    public GameObject playerSelectionPrefab;

    #region Tooltip
    [Tooltip("填入玩家细节列表")]
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
