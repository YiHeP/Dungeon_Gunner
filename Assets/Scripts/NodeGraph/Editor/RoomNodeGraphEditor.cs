using UnityEditor.Callbacks;
using UnityEngine;
using UnityEditor;
using UnityEditor.MPE;
using System.Collections.Generic;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeGUIStyle;
    private GUIStyle roomNodeSelectedStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;

    private Vector2 graphOffset;
    private Vector2 grapgDrag;

    private RoomNodeSO currentRoomNode = null;
    private RoomNodeTypeListSO roomNodeTypeList;

    private const float nodeWidth = 160f;
    private const float nodeHeight = 75;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;
    private const float connectingLineWidth = 3f;
    private const float connectingLineArrowSize = 6f;

    private const float gridLarge = 100f;
    private const float gridSmall = 25f;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph/Editor")]

    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("RoomNodeGraph");
    }

    private void OnEnable()
    {
        Selection.selectionChanged += InspectorSelectionChanged;

        roomNodeGUIStyle = new GUIStyle();
        roomNodeGUIStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeGUIStyle.normal.textColor = Color.white;
        roomNodeGUIStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeGUIStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        roomNodeSelectedStyle = new GUIStyle();
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        roomNodeSelectedStyle.normal.textColor = Color.white;
        roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        roomNodeTypeList = GameResources.Instance.roomNodeTypelist;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= InspectorSelectionChanged;
    }

    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;
        if (roomNodeGraph != null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }

        return false;
    }

    public void OnGUI()
    {

        if (currentRoomNodeGraph != null)
        {
            DrawBackGroundGrid(gridSmall, 0.2f, Color.gray);
            DrawBackGroundGrid(gridLarge, 0.3f, Color.gray);

            DrawDraggedLine();

            ProcessEvents(Event.current);

            DrawRoomConnections();

            DrawRoomNode();
        }

        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawBackGroundGrid(float gridSize, float gridOpacity, Color gridColor)
    {
        int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
        int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);

        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
        graphOffset += grapgDrag * 0.5f;

        Vector3 gridOffset = new Vector3(graphOffset.x % gridSize, graphOffset.y % gridSize, 0);

        for(int i = 0;i< verticalLineCount;i++)
        {
            Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0f) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);
        }

        for(int i = 0; i< horizontalLineCount;i++)
        {
            Handles.DrawLine(new Vector3(-gridSize , gridSize * i , 0f) + gridOffset, new Vector3(position.width + gridSize , gridSize * i , 0f) + gridOffset);
        }

        Handles.color = Color.white;

    }

    private void DrawDraggedLine()
    {
        if(currentRoomNodeGraph.linePoisition != Vector2.zero)
        {
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePoisition,
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePoisition, Color.white, null,
                connectingLineWidth);
        }
    }

    private RoomNodeSO IsMouseOverRoomNode(Event e)
    {
        for(int i = currentRoomNodeGraph.roomNodeList.Count - 1 ; i >= 0 ; i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(e.mousePosition))
            {
                return currentRoomNodeGraph.roomNodeList[i];
            }
        }

        return null;
    }

    private void ProcessEvents(Event e)
    {
        grapgDrag = Vector2.zero;

        if (currentRoomNode == null || currentRoomNode.isLeftDragging == false)
        {
            currentRoomNode = IsMouseOverRoomNode(e);
        }
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(e);
        }
        else
        {
            currentRoomNode.ProcessEvents(e);
        }
    }

    private void ProcessRoomNodeGraphEvents(Event e)
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
            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event e)
    {
        if(e.button == 1)
        {
            ShowContextMenu(e.mousePosition);
        }
        else if(e.button == 0)
        {
            ClearLineDrag();
            ClearAllSelectedRoomNodes();
        }
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("创建房间节点"), false, CreakRoomNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("选择所有房间节点"), false, SelectAllRoomNode);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("删除所有选中的房间节点链接"), false, DeleteSelectedRoomNodeLinks);
        menu.AddItem(new GUIContent("删除所有选中的房间节点"), false, DeleteSelectedRoomNode);
        menu.ShowAsContext();
    }

    private void CreakRoomNode(object mousePoisitionObject)
    {
        if(currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            CreakRoomNode(new Vector2(200, 200), roomNodeTypeList.list.Find(x => x.isEntrance));
        }

        CreakRoomNode(mousePoisitionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    private void CreakRoomNode(object mousePoisitionObject,RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePoisition = (Vector2)mousePoisitionObject;

        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        roomNode.Initialise(new Rect(mousePoisition, new Vector2(nodeWidth, nodeHeight)),currentRoomNodeGraph,roomNodeType);

        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();

        currentRoomNodeGraph.OnValidate();
    }

    private void DeleteSelectedRoomNode()
    {
        Queue<RoomNodeSO> roomNodeDeleteQueue = new Queue<RoomNodeSO>();
        foreach(RoomNodeSO node in currentRoomNodeGraph.roomNodeList)
        {
            if(node.isSelected && !node.roomNodeType.isEntrance)
            {
                roomNodeDeleteQueue.Enqueue(node);
                foreach(string childRoomId in node.childRoomNodeIDList)
                {
                    RoomNodeSO childNode = currentRoomNodeGraph.GetRoomNode(childRoomId);
                    if(childNode != null)
                    {
                        childNode.RemoveParentRoomNodeIDFromRoomNode(node.id);
                    }
                }

                foreach(string parentId in node.parentRoomNodeIDList)
                {
                    RoomNodeSO parentNode = currentRoomNodeGraph.GetRoomNode(parentId);
                    if(parentNode != null)
                    {
                        parentNode.RemoveChildRoomNodeIDFromRoomNode(node.id);
                    }
                }
            }
        }

        while(roomNodeDeleteQueue.Count > 0)
        {
            RoomNodeSO roomNodeToDelete = roomNodeDeleteQueue.Dequeue();

            currentRoomNodeGraph.roomNodeDictionary.Remove(roomNodeToDelete.id);

            currentRoomNodeGraph.roomNodeList.Remove(roomNodeToDelete);

            DestroyImmediate(roomNodeToDelete,true);

            AssetDatabase.SaveAssets();
        }
    }

    private void DeleteSelectedRoomNodeLinks()
    {
        foreach(RoomNodeSO node in currentRoomNodeGraph.roomNodeList)
        {
            if(node.isSelected && node.childRoomNodeIDList.Count > 0)
            {
                for(int i = node.childRoomNodeIDList.Count - 1; i >= 0; i--)
                {
                    RoomNodeSO childNode = currentRoomNodeGraph.GetRoomNode(node.childRoomNodeIDList[i]);
                    if(childNode != null && childNode.isSelected)
                    {
                        node.RemoveChildRoomNodeIDFromRoomNode(childNode.id);
                        childNode.RemoveParentRoomNodeIDFromRoomNode(node.id);
                    }
                }
            }
        }
        ClearAllSelectedRoomNodes();
    }

    private void ClearAllSelectedRoomNodes()
    {
        foreach(RoomNodeSO node in currentRoomNodeGraph.roomNodeList)
        {
            if(node.isSelected)
            {
                node.isSelected = false;
                GUI.changed = true;
            }
        }
    }

    private void SelectAllRoomNode()
    {
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = true;
        }
        GUI.changed = true;
    }

    private void ProcessMouseDragEvent(Event e)
    {
        if (e.button == 1)
        {
            ProcessRightMouseDragEvent(e);
        }
        else if (e.button == 0)
        {
            ProcessLeftMouseDragEvent(e.delta);
        }
    }

    private void ProcessRightMouseDragEvent(Event e)
    {
        if(currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            DragConnectingLine(e.delta);
            GUI.changed = true;
        }
    }

    private void ProcessLeftMouseDragEvent(Vector2 delta)
    {
        grapgDrag = delta;
        for(int i = 0; i < currentRoomNodeGraph.roomNodeList.Count; i++)
        {
            currentRoomNodeGraph.roomNodeList[i].DragNode(delta);
        }
        GUI.changed = true;
    }

    private void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePoisition += delta;
    }

    private void ProcessMouseUpEvent(Event e)
    {
        if(e.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            RoomNodeSO roomNode = IsMouseOverRoomNode(e);
            if(roomNode != null)
            {
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }

            ClearLineDrag();
        }
    }

    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePoisition = Vector2.zero;
        GUI.changed = true;
    }

    private void DrawRoomConnections()
    {
        foreach(RoomNodeSO node in currentRoomNodeGraph.roomNodeList)
        {
            if(node.childRoomNodeIDList.Count > 0)
            {
                foreach(string childRoomNodeId in node.childRoomNodeIDList)
                {
                    DrawConnectionsLine(node, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeId]);
                    GUI.changed = true;
                }
            }
        }
    }

    private void DrawConnectionsLine(RoomNodeSO sNode , RoomNodeSO eNode)
    {
        Vector2 sPoisition = sNode.rect.center;
        Vector2 ePoisition = eNode.rect.center;
        Vector2 mPoisition = (sPoisition + ePoisition)/2f;
        Vector2 direction = ePoisition - sPoisition;

        Vector2 arrowPoisition1 = mPoisition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowPoisition2 = mPoisition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowHeadPoisition = mPoisition + direction.normalized * connectingLineArrowSize;

        Handles.DrawBezier(arrowHeadPoisition, arrowPoisition1, arrowHeadPoisition, arrowPoisition1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoisition, arrowPoisition2, arrowHeadPoisition, arrowPoisition2, Color.white, null, connectingLineWidth);

        Handles.DrawBezier(sPoisition, ePoisition, sPoisition, ePoisition, Color.white, null, connectingLineWidth);
        GUI.changed = true;
    }

    private void DrawRoomNode()
    {
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected)
            {
                roomNode.Draw(roomNodeSelectedStyle);
            }
            else
            {
                roomNode.Draw(roomNodeGUIStyle);
            }
        }

        GUI.changed = true;
    }

    private void InspectorSelectionChanged()
    {
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;
        if (roomNodeGraph != null)
        {
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }

}
