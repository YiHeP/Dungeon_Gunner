using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string id;
    public string templateid;
    public GameObject prefab;
    public RoomNodeTypeSO roomNodeType;
    public Vector2Int lowerBounds;//世界坐标系中的左下角位置
    public Vector2Int upperBounds;
    public Vector2Int templateLowerBounds;//模板房间的左下角坐标，相对于模板本身的局部坐标系
    public Vector2Int templateUpperBounds;
    public Vector2Int[] spawnPositionArray;
    public MusicTrackSO battleMusic;
    public MusicTrackSO ambientMusic;
    public List<SpawnableObjectByLevel<EnemyDetailsSO>> enemiesByLevelList;
    public List<RoomEnemySpawnParameters> roomEnemySpawnParametersList;
    public List<string> childRoomIdList;
    public string parentRoomId;
    public List<Doorway> doorWayList;
    public bool isPositioned = false;
    public InstantiatedRoom instantiatedRoom;
    public bool isLit = false;
    public bool isCleanedOfEnemies = false;
    public bool isPreviouslyVisited = false;

    public Room()
    {
        childRoomIdList = new List<string>();
        doorWayList = new List<Doorway>();
    }

    public int GetNumberOfEnemiesToSpwan(DungeonLevelSO dungeonLevel)
    {
        foreach(RoomEnemySpawnParameters roomEnemySpawnParameters in roomEnemySpawnParametersList)
        {
            if(roomEnemySpawnParameters.dungeonLevel == dungeonLevel)
            {
                return Random.Range(roomEnemySpawnParameters.minTotalEnemiesToSpawn, roomEnemySpawnParameters.maxTotalEnemiesToSpawn);
            }
        }
        return 0;
    }

    public RoomEnemySpawnParameters GetRoomEnemySpawnParameters(DungeonLevelSO dungeonLevel)
    {
        foreach (RoomEnemySpawnParameters roomEnemySpawnParameters in roomEnemySpawnParametersList)
        {
            if (roomEnemySpawnParameters.dungeonLevel == dungeonLevel)
            {
                return roomEnemySpawnParameters;
            }
        }
        return null;
    }
}
