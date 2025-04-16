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
