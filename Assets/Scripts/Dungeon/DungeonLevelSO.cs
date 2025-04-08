using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel", menuName = "Scriptable Objects/Dungeon/地牢关卡")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header 关卡基础细节
    [Space(10)]
    [Header("关卡基础细节")]
    #endregion Header 关卡基础细节

    #region Tooltip
    [Tooltip("关卡名")]
    #endregion Tooltip

    public string levelName;

    #region 关卡的房间模板
    [Space(10)]
    [Header("关卡的房间模板")]
    #endregion

    public List<RoomTemplateSO> roomTemplateList;

    #region Header 关卡的房间节点图
    [Space(10)]
    [Header("关卡的房间节点图")]
    #endregion Header 关卡的房间节点图
    #region Tooltip
    [Tooltip("关卡从此处进行选择房间节点图")]
    #endregion Tooltip

    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEmptyString(this,nameof(levelName),levelName);
        if (HelpUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;
        if (HelpUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return;

        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;

        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (roomTemplate == null) 
                return;
            if (roomTemplate.roomNodeType.isCorridorEW)
                isEWCorridor = true;
            if(roomTemplate.roomNodeType.isCorridorNS)
                isNSCorridor = true;
            if(roomTemplate.roomNodeType.isEntrance)
                isEntrance = true;
        }

        if (!isEWCorridor)
        {
            Debug.Log("在" + this.name.ToString() + "无东西走向走廊");
        }
        if (!isNSCorridor)
        {
            Debug.Log("在" + this.name.ToString() + "无南北走向走廊");
        }
        if (!isEntrance)
        {
            Debug.Log("在" + this.name.ToString() + "无入口");
        }

        foreach (RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;
            foreach(RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if(roomNodeSO == null) 
                    continue;
                if (roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS ||
                    roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isEntrance)
                    continue;
                bool isRoomNodeTypeFound = false;

                foreach(RoomTemplateSO roomTemplate in roomTemplateList)
                {
                    if (roomTemplate == null) 
                        continue;
                    if(roomNodeSO.roomNodeType ==  roomTemplate.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }
                if(!isRoomNodeTypeFound)
                {
                    Debug.Log("在"+this.name.ToString()+"中没有在"+roomNodeGraph.name.ToString()+"中找到以下被记录的房间模板"+
                        roomNodeSO.roomNodeType.name.ToString());
                }
            }
        }
    }

#endif
    #endregion Validation
}
