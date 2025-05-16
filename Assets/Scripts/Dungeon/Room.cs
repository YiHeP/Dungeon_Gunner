using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public string id;
    public string templateid;
    public GameObject prefab;
    public RoomNodeTypeSO roomNodeType;
    public Vector2Int lowerBounds;//��������ϵ�е����½�λ��
    public Vector2Int upperBounds;
    public Vector2Int templateLowerBounds;//ģ�巿������½����꣬�����ģ�屾��ľֲ�����ϵ
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
