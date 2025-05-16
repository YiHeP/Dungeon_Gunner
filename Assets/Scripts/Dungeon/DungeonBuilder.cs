using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DungeonBuilder : SingletonMonobehaviour<DungeonBuilder>
{
    public Dictionary<string, Room> dungeonBuilderRoomDictionary = new Dictionary<string,Room>();
    private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string,RoomTemplateSO>();
    private List<RoomTemplateSO> roomTemplateList = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    private bool dungeonBuilderSuccessful;

    private void OnEnable()
    {
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 0f);
    }

    private void OnDisable()
    {
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 1f);
    }

    protected override void Awake()
    {
        base.Awake();
        //读取所有房间类型
        LoadRoomNodeTypeList();
    }

    //读取房间类型列表
    private void LoadRoomNodeTypeList()
    {
        roomNodeTypeList = GameResources.Instance.roomNodeTypelist;
    }

    //生成地牢
    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomTemplateList = currentDungeonLevel.roomTemplateList;

        LoadRoomTemplateIntoDictionary();//加载房间模块

        dungeonBuilderSuccessful = false;

        int dungeonBuildAttempts = 0;//建造次数

        while(!dungeonBuilderSuccessful || dungeonBuildAttempts < Settings.maxDungeonBuildAttempts)
        {
            dungeonBuildAttempts++;

            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGrpah(currentDungeonLevel.roomNodeGraphList);

            int dungeonRebuilderAttemptsForNodeGraph = 0;
            dungeonBuilderSuccessful = false;
            while(!dungeonBuilderSuccessful || dungeonRebuilderAttemptsForNodeGraph < Settings.maxDungeonRebuildAttemptsForRoomNodeGraph)
            {
                ClearDungeon();

                dungeonRebuilderAttemptsForNodeGraph++;

                dungeonBuilderSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
            }

            if (dungeonBuilderSuccessful)
            {
                InstantiateRoomGameobjects();
            }

        }

        return dungeonBuilderSuccessful;

    }

    //将房间模板加载到字典中
    private void LoadRoomTemplateIntoDictionary()
    {
        roomTemplateDictionary.Clear();

        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if(!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log("有重复的房间模板在" + roomTemplateList);
            }
        }
    }

    //尝试建造随机地牢
    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(x => x.isEntrance));
        if(entranceNode != null)
        {
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            Debug.Log("无入口");
            return false;
        }
        bool noRoomOverlaps = true;

        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue,noRoomOverlaps);

        if(openRoomNodeQueue.Count == 0 && noRoomOverlaps)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //开放室节点队列中的处理室
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph,Queue<RoomNodeSO> openRoomNodeQueue, bool noRoomOverlaps)
    {
        while(openRoomNodeQueue.Count > 0 && noRoomOverlaps)
        {
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();

            foreach(RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNode(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }

            if(roomNode.roomNodeType.isEntrance)
            {
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
                Room room = CreateRoomFromRoonTemplate(roomTemplate, roomNode);
                room.isPositioned = true;
                dungeonBuilderRoomDictionary.Add(room.id,room);
            }
            else
            {
                Room parentRoom = dungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];

                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }
        }
        return noRoomOverlaps;
    }

    //是否可以放置没有重叠的房间
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
    {
        bool RoomOverlaps = true;
        while(RoomOverlaps)
        {
            List<Doorway> unconnectedAvailableParentDoorWays = GetUnconnectedAvailableDoorWays(parentRoom.doorWayList).ToList();

            if(unconnectedAvailableParentDoorWays.Count == 0)
            {
                return false;
            }

            Doorway doorWayParent = unconnectedAvailableParentDoorWays[UnityEngine.Random.Range(0,unconnectedAvailableParentDoorWays.Count)];

            RoomTemplateSO roomTemplate = GetRandomTemplateForRoomConsistentWithParent(roomNode, doorWayParent);

            Room room = CreateRoomFromRoonTemplate(roomTemplate, roomNode);

            if(PlaceTheRoom(parentRoom,doorWayParent,room))
            {
                RoomOverlaps = false;
                room.isPositioned = true;
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                RoomOverlaps = true;
            }
        }
        return true;
    }

    //放置房间
    private bool PlaceTheRoom(Room parentRoom,Doorway doorwayParent,Room room)
    {
        Doorway doorWay = GetOppositeDoorWay(doorwayParent, room.doorWayList);
        if(doorWay == null)
        {
            doorwayParent.isUnavailable = false;
            return false;
        }

        Vector2Int parentDoorWayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;

        Vector2Int adjustment = Vector2Int.zero;
        switch(doorWay.orientation)
        {
            case Orientation.east:
                adjustment = new Vector2Int(-1,0);
                break;
            case Orientation.west:
                adjustment = new Vector2Int(1, 0);
                break;
            case Orientation.north:
                adjustment = new Vector2Int(0, -1);
                break;
            case Orientation.south:
                adjustment = new Vector2Int(0, 1);
                break;
            case Orientation.none:
                break;
            default:
                break;
        }
        room.lowerBounds = parentDoorWayPosition + adjustment + room.templateLowerBounds - doorWay.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        Room overlappingRoom = CheckForRoomOverlap(room);
        if(overlappingRoom == null)
        {
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;

            doorWay.isUnavailable = true;
            doorWay.isConnected = true;
            return true;
        }
        else
        {
            doorwayParent.isUnavailable=true;
            return false;
        }

    }

    //获取对面门
    private Doorway GetOppositeDoorWay(Doorway doorWayParent, List<Doorway> doorWays)
    {
        foreach(Doorway doorway in doorWays)
        {
            if(doorWayParent.orientation == Orientation.east && doorway.orientation == Orientation.west)
            {
                return doorway;
            }
            else if(doorWayParent.orientation == Orientation.west && doorway.orientation == Orientation.east)
            {
                return doorway;
            }
            else if(doorWayParent.orientation == Orientation.north && doorway.orientation == Orientation.south)
            {
                return doorway;
            }
            else if(doorWayParent.orientation == Orientation.south && doorway.orientation == Orientation.north)
            {
                return doorway;
            }
        }
        return null;
    }

    //检查
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach(KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            if(room.id == roomToTest.id || !room.isPositioned)
            {
                continue;
            }
            if(IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }
        return null;
    }

    //房间是否重叠
    private bool IsOverLappingRoom(Room room1,Room room2)
    {
        bool isOverlappingX = IsOverLappingInterval(room1.lowerBounds.x,room1.upperBounds.x,room2.lowerBounds.x,room2.upperBounds.x);
        bool isOverlappingY = IsOverLappingInterval(room1.lowerBounds.y,room1.upperBounds.y,room2.lowerBounds.y,room2.upperBounds.y);

        if(isOverlappingX && isOverlappingY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsOverLappingInterval(int imin1,int imax1,int imin2,int imax2)
    {
        if(Mathf.Max(imin1,imin2) <= Mathf.Min(imax1,imax2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //获取与父级一致的房间的随机模板
    private RoomTemplateSO GetRandomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode,Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;

        if(roomNode.roomNodeType.isCorridor)
        {
            switch(doorwayParent.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNS));
                    break;
                case Orientation.east:
                case Orientation.west:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;
                case Orientation.none:
                    break;
                default:
                    break;
            }
        }
        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }
        return roomTemplate;
    }

    //获取随机房间模板
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();
        //获取模板
        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if(roomTemplate.roomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }
        if(matchingRoomTemplateList.Count == 0)
            return null;
        return matchingRoomTemplateList[UnityEngine.Random.Range(0,matchingRoomTemplateList.Count)];
    }

    //选择随机房间节点图
    private RoomNodeGraphSO SelectRandomRoomNodeGrpah(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if(roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0,roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("列表中无房间图");
            return null;
        }
    }

    //获取未连接的可用门口
    private IEnumerable<Doorway> GetUnconnectedAvailableDoorWays(List<Doorway> roomDoorWayList)
    {
        foreach(Doorway doorway in roomDoorWayList)
        {
            if(!doorway.isConnected && !doorway.isUnavailable)
            {
                yield return doorway;
            }
        }
    }

    //通过房间模板创建房间
    private Room CreateRoomFromRoonTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        Room room = new Room();
        room.templateid = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.roomNodeType = roomTemplate.roomNodeType;
        room.lowerBounds = roomTemplate.lowerBounds;
        room.upperBounds = roomTemplate.upperBounds;
        room.spawnPositionArray = roomTemplate.spawnPositionArray;
        room.templateLowerBounds = roomTemplate.lowerBounds;
        room.templateUpperBounds = roomTemplate.upperBounds;
        room.enemiesByLevelList = roomTemplate.enemiesByLevelList;
        room.roomEnemySpawnParametersList = roomTemplate.roomEnemySpawnParameterList;
        room.battleMusic = roomTemplate.battleMusic;
        room.ambientMusic = roomTemplate.ambientMusic;

        room.childRoomIdList = CopyStringList(roomNode.childRoomNodeIDList);
        room.doorWayList = CopyDoowWayList(roomTemplate.doorwayList);

        if(roomNode.parentRoomNodeIDList.Count == 0)
        {
            room.parentRoomId = "";
            room.isPreviouslyVisited = true;
            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.parentRoomId = roomNode.parentRoomNodeIDList[0];
        }

        if(room.GetNumberOfEnemiesToSpwan(GameManager.Instance.GetCurrentDungeonLevel()) == 0)
        {
            room.isCleanedOfEnemies = true;
        }

        return room;
    }

    //深拷贝子房间节点id
    private List<string> CopyStringList(List<string> oldList)
    {
        List<string> newList = new List<string>();

        foreach(string item in oldList)
        {
            newList.Add(item);
        }

        return newList;
    }

    //深拷贝门口
    private List<Doorway> CopyDoowWayList(List<Doorway> oldDoorWayList)
    {
        List<Doorway> newDoorWayList = new List<Doorway>();

        foreach(Doorway doorway in oldDoorWayList)
        {
            Doorway newDoorWay = new Doorway();

            newDoorWay.position = doorway.position;
            newDoorWay.orientation = doorway.orientation;
            newDoorWay.doorPrefab = doorway.doorPrefab;
            newDoorWay.isConnected = doorway.isConnected;
            newDoorWay.isUnavailable = doorway.isUnavailable;
            newDoorWay.doorwayStartCopyPosition = doorway.doorwayStartCopyPosition;
            newDoorWay.doorwayCopyTileHeight = doorway.doorwayCopyTileHeight;
            newDoorWay.doorwayCopyTileWidth = doorway.doorwayCopyTileWidth;

            newDoorWayList.Add(newDoorWay);
        }
        return newDoorWayList;
    }
    //实例化房间物体
    private void InstantiateRoomGameobjects()
    {
        foreach(KeyValuePair<string,Room> keyvaluepair in dungeonBuilderRoomDictionary)
        {
            Room room = keyvaluepair.Value;
            Vector3 roomPoisition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);
            GameObject roomObject = Instantiate(room.prefab, roomPoisition, Quaternion.identity, transform);
            InstantiatedRoom instantiatedRoom = roomObject.GetComponentInChildren<InstantiatedRoom>();
            instantiatedRoom.room = room;
            instantiatedRoom.Initialise(roomObject);
            room.instantiatedRoom = instantiatedRoom;

        }
    }

    public RoomTemplateSO GetRoomTemplate(string roomTeplateId)
    {
        if(roomTemplateDictionary.TryGetValue(roomTeplateId, out RoomTemplateSO roomTemplate))
        {
            return roomTemplate;
        }
        else
        {
            return null;
        }
    }

    public Room GetRoomByRoomId(string roomId)
    {
        if(dungeonBuilderRoomDictionary.TryGetValue(roomId, out Room room))
        { 
            return room; 
        }
        else
        { 
            return null; 
        }
    }

    //清理地牢
    private void ClearDungeon()
    {
        if(dungeonBuilderRoomDictionary.Count > 0)
        {
            foreach(KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
            {
                Room room = keyValuePair.Value;
                if(room.instantiatedRoom != null)
                {
                    Destroy(room.instantiatedRoom.gameObject);
                }

            }
            dungeonBuilderRoomDictionary.Clear();
        }
    }

}
