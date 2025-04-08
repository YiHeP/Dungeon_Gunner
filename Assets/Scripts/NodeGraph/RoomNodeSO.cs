using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;

public class RoomNodeSO : ScriptableObject
{
    public string id;
    public List<string> parentRoomNodeIDList = new List<string>();
    public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    #region Unity_Editor
#if UNITY_EDITOR
    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftDragging = false;
    [HideInInspector] public bool isSelected = false;
    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        roomNodeTypeList = GameResources.Instance.roomNodeTypelist;
    }

    public void Draw(GUIStyle nodeStyle)
    {
        GUILayout.BeginArea(rect, nodeStyle);

        EditorGUI.BeginChangeCheck();

        if(parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }
        else
        {
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypeToDisplay());

            roomNodeType = roomNodeTypeList.list[selection];

            if (roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor ||!roomNodeTypeList.list
                [selected].isCorridor && roomNodeTypeList.list[selection].isCorridor || !roomNodeTypeList.list[selected].isBossRoom
                && roomNodeTypeList.list[selection].isBossRoom)
            {
                if (childRoomNodeIDList.Count > 0)
                {
                    for (int i = childRoomNodeIDList.Count - 1; i >= 0; i--)
                    {
                        RoomNodeSO childNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);

                        if (childNode != null)
                        {
                            RemoveChildRoomNodeIDFromRoomNode(childNode.id);
                            childNode.RemoveParentRoomNodeIDFromRoomNode(id);
                        }
                    }
                }
            }
        }

        if(EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(this);
        }

        GUILayout.EndArea();
    }

    public string[] GetRoomNodeTypeToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for(int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }
        return roomArray;
    }

    public void ProcessEvents(Event e)
    {
        switch(e.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(e);
                break;
            case EventType.MouseUp:
                ProcessMouseUpEvent(e);
                break;
            case EventType.MouseDrag:
                ProcessMouseDragEvent(e);
                break;
        }
    }

    public void ProcessMouseDownEvent(Event e)
    {
        if(e.button == 0)
        {
            ProcessLeftClickDownEvent();
        }
        else if(e.button == 1)
        {
            ProcessRightClickDownEvent(e);
        }

    }

    public void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;

        if(isSelected == true)
        {
            isSelected = false;
        }
        else
        {
            isSelected = true;
        }
    }

    public void ProcessRightClickDownEvent(Event e)
    {
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, e.mousePosition);
    }

    public void ProcessMouseUpEvent(Event e)
    {
        if (e.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }

    public void ProcessLeftClickUpEvent()
    {
        if(isLeftDragging)
        {
            isLeftDragging = false;
        }
    }

    public void ProcessMouseDragEvent(Event e)
    {
        if(e.button == 0)
        {
            ProcessLeftClickDragEvent(e);
        }
    }

    public void ProcessLeftClickDragEvent(Event e)
    {
        isLeftDragging = true;
        DragNode(e.delta);
        GUI.changed = true;
    }

    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }

    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        if(IsChildRoomValid(childID))
        {
            childRoomNodeIDList.Add(childID);
            return true;
        }
        return false;
    }

    public bool IsChildRoomValid(string childID)
    {
        bool IsBossNodeAlready = false;

        foreach(RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            if(roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
            {
                IsBossNodeAlready = true;
            }
        }

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && IsBossNodeAlready)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false; 

        if(childRoomNodeIDList.Contains(childID))
            return false;

        if(id == childID)
            return false;

        if(parentRoomNodeIDList.Contains(childID) )
            return false;

        if(roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;

        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count >= Settings.MaxChildCorridors)
            return false;

        if(roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;

        if(!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count>0)
            return false;

        return true;
    }

    public bool AddParentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }

    public bool RemoveChildRoomNodeIDFromRoomNode(string childId)
    {
        if(childRoomNodeIDList.Contains(childId))
        {
            childRoomNodeIDList.Remove(childId);
            return true;
        }
        return false;
    }

    public bool RemoveParentRoomNodeIDFromRoomNode(string parentId)
    {
        if (parentRoomNodeIDList.Contains(parentId))
        {
            parentRoomNodeIDList.Remove(parentId);
            return true;
        }
        return false;
    }

#endif
    #endregion
}
