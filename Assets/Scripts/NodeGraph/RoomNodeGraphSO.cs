using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeGraph",menuName ="Scriptable Objects/room/房间节点图")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }

    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();
        foreach(RoomNodeSO roomNode in roomNodeList)
        {
            roomNodeDictionary[roomNode.id] = roomNode;
        }
    }

    public RoomNodeSO GetRoomNode(RoomNodeTypeSO roomNodeType)
    {
        foreach(RoomNodeSO roomNode in roomNodeList)
        {
            if(roomNode.roomNodeType == roomNodeType) 
                return roomNode;
        }
        return null;
    }

    public RoomNodeSO GetRoomNode(string id)
    {
        if(roomNodeDictionary.TryGetValue(id,out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
    }

    public IEnumerable<RoomNodeSO> GetChildRoomNode(RoomNodeSO parentNode)
    {
        foreach(string childNodeId in parentNode.childRoomNodeIDList)
        {
            yield return GetRoomNode(childNodeId);
        }
    }

    #region Editor
#if UNITY_EDITOR
    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom;
    [HideInInspector] public Vector2 linePoisition;

    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO roomNode,Vector2 poisition)
    {
        roomNodeToDrawLineFrom = roomNode;
        linePoisition = poisition;
    }
#endif
#endregion
}
