using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Dungeon/房间")]
public class RoomTemplateSO : ScriptableObject
{
    [HideInInspector] public string guid;

    #region Header房间预制体

    [Space(10)]
    [Header("房间预制体")]

    #endregion Header 房间预制体

    #region Tooltip

    [Tooltip("房间的游戏对象预制件（这将包含房间和环境游戏对象的所有瓦片地图）")]

    #endregion Tooltip

    public GameObject prefab;

    [HideInInspector] public GameObject previousPrefab; // 这用于在复制 SO 并更改预制件时重新生成 GUID


    #region Header 房间配置

    [Space(10)]
    [Header("房间配置")]

    #endregion Header 房间配置

    #region Tooltip

    [Tooltip("The room node type SO. The room node types correspond to the room nodes used in the room node graph.  The exceptions being with corridors.  In the room node graph there is just one corridor type 'Corridor'.  For the room templates there are 2 corridor node types - CorridorNS and CorridorEW.")]

    #endregion Tooltip

    public RoomNodeTypeSO roomNodeType;

    #region Tooltip

    [Tooltip("如果你想象一个围绕 room tilemap 的矩形，它刚好完全包围它，那么 room 下边界表示该矩形的左下角。这应该从房间的瓦片地图中确定（使用坐标画笔指针获取左下角的瓦片地图网格位置（注意：这是本地瓦片地图位置，而不是世界位置")]

    #endregion Tooltip

    public Vector2Int lowerBounds;

    #region Tooltip

    [Tooltip("如果你想象一个围绕 room tilemap 的矩形，它刚好完全包围它，那么 room 的上限代表该矩形的右上角。这应该从房间的瓦片地图中确定（使用坐标画笔指针获取右上角的瓦片地图网格位置（注意：这是本地瓦片地图位置，而不是世界位置")]

    #endregion Tooltip

    public Vector2Int upperBounds;

    #region Tooltip

    [Tooltip("一个房间最多应有四个门口 - 每个指南针方向一个。这些应该具有一致的 3 个图块开口大小，中间的图块位置是门口坐标“位置”")]

    #endregion Tooltip

    [SerializeField] public List<Doorway> doorwayList;

    #region Tooltip

    [Tooltip("瓦片地图坐标中房间的每个可能的生成位置（用于敌人和箱子）都应该添加到此数组中")]

    #endregion Tooltip

    public Vector2Int[] spawnPositionArray;

    public List<Doorway> GetDoorwayList()
    {
        return doorwayList;
    }

    #region Validation

#if UNITY_EDITOR

    //验证 SO 字段
    private void OnValidate()
    {
        //如果为空或预制件更改，则设置唯一 GUID
        if (guid == "" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);
        }

        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(doorwayList), doorwayList);

        //检查填充的生成位置
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPositionArray), spawnPositionArray);
    }

#endif

    #endregion Validation
}