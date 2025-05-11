using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap;
    [HideInInspector] public int[,] aStarMovementPenalty;
    [HideInInspector] public Bounds roomCollisionBounds;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        roomCollisionBounds = boxCollider2D.bounds;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Settings.playerTag && room != GameManager.Instance.GetCurrentRoom())
        {
            this.room.isPreviouslyVisited = true;

            StaticEventHandler.CallRoomChangedEvent(room);
        }
    }

    public void Initialise(GameObject roomGameObject)
    {
        PopulateTilemapMemberVariable(roomGameObject);

        BlockOffUnuseDoorWays();

        AddObstaclesAndPerferredPaths();

        AddDoorToRoom();

        DisableCollisionTilemapRenderer();
    }

    //填充瓦片地图成员变量
    private void PopulateTilemapMemberVariable(GameObject roomGameObject)
    {
        grid = roomGameObject.GetComponentInChildren<Grid>();

        Tilemap[] tilemaps = roomGameObject.GetComponentsInChildren<Tilemap>();

        foreach(Tilemap tilemap in tilemaps)
        {
            if(tilemap.tag == "groundTilemap")
            {
                groundTilemap = tilemap;
            }
            else if(tilemap.tag == "decoration1Tilemap")
            {
                decoration1Tilemap = tilemap;
            }
            else if(tilemap.tag == "decoration2Tilemap")
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.tag == "frontTilemap")
            {
                frontTilemap = tilemap;
            }
            else if (tilemap.tag == "collisionTilemap")
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.tag == "minimapTilemap")
            {
                minimapTilemap = tilemap;
            }
        }
    }

    //封锁未使用的门道
    private void BlockOffUnuseDoorWays()
    {
        foreach(Doorway doorway in room.doorWayList)
        {
            if(doorway.isConnected)
            {
                continue;
            }

            if(collisionTilemap != null)
            {
                BlockADoorWayOnTilemapLayer(collisionTilemap,doorway);
            }

            if(frontTilemap != null)
            {
                BlockADoorWayOnTilemapLayer(frontTilemap,doorway);
            }

            if(groundTilemap != null)
            {
                BlockADoorWayOnTilemapLayer(groundTilemap,doorway);
            }

            if(decoration1Tilemap != null)
            {
                BlockADoorWayOnTilemapLayer(decoration1Tilemap,doorway);
            }

            if(decoration2Tilemap != null)
            {
                BlockADoorWayOnTilemapLayer(decoration2Tilemap,doorway);
            }

            if(minimapTilemap != null)
            {
                BlockADoorWayOnTilemapLayer(minimapTilemap,doorway);
            }
        }
    }

    private void BlockADoorWayOnTilemapLayer(Tilemap tilemap,Doorway doorway)
    {
        switch(doorway.orientation)
        {
            case Orientation.south:
            case Orientation.north:
                BlockDoorWayHorizontally(tilemap,doorway);
                break;
            case Orientation.west:
            case Orientation.east:
                BlockDoorWayVertically(tilemap, doorway);
                break;
            case Orientation.none:
                break;
        }
    }

    private void BlockDoorWayHorizontally(Tilemap tilemap,Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;
        for(int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for(int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                Matrix4x4 matrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos,0), tilemap.GetTile(new Vector3Int(startPosition.x
                    + xPos,startPosition.y - yPos, 0)));

                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), matrix);
            }
        }
    }

    private void BlockDoorWayVertically(Tilemap tilemap,Doorway doorway)
    {
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;
        for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
        {
            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                Matrix4x4 matrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));

                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y -1 - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x
                    + xPos, startPosition.y - yPos, 0)));

                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y -1 - yPos, 0), matrix);
            }
        }
    }

    private void AddObstaclesAndPerferredPaths()//设置移动惩罚
    {
        aStarMovementPenalty = new int[room.templateUpperBounds.x -  room.templateLowerBounds.x + 1 , room.templateUpperBounds.y - 
            room.templateLowerBounds.y + 1];
        for(int x = 0; x<(room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for(int y = 0; y<(room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;
                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x,y + room.templateLowerBounds.y,0));

                foreach(TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                {
                    if(tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                if(tile == GameResources.Instance.perferredEnemyPathTile)
                {
                    aStarMovementPenalty[x, y] = Settings.perferredPathAStarMovementPenalty;
                }
            }
        }
    }

    private void AddDoorToRoom()
    {
        if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS) return;

        foreach(Doorway doorway in room.doorWayList)
        {
            if(doorway.doorPrefab != null && doorway.isConnected)
            {
                float tileDistance = Settings.tileSizePixels / Settings.pixelsPerUnit;
                GameObject door = null;

                if(doorway.orientation == Orientation.north)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance, 0f);
                }
                else if(doorway.orientation == Orientation.south)
                {
                    door = Instantiate(doorway.doorPrefab,gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y,0f);
                }
                else if(doorway.orientation == Orientation.east)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance, doorway.position.y + tileDistance * 1.25f,0f);
                }
                else if(doorway.orientation == Orientation.west)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x, doorway.position.y + tileDistance *1.25f , 0f);
                }

                Door doorComponent = door.GetComponent<Door>();
                if(room.roomNodeType.isBossRoom)
                {
                    doorComponent.isBossRoomDoor = true;
                    doorComponent.lockDoor();//玩家通过其他所有关卡才能打开boss房
                }
            }
        }
    }

    //禁用 Collision Tilemap Renderer
    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;

    }

    public void DisableRoomCollider()
    {
        boxCollider2D.enabled = false;
    }

    public void EnableRoomCollider()
    {
        boxCollider2D.enabled = true;
    }

    public void LockDoors()
    {
        Door[] doorArray = GetComponentsInChildren<Door>();

        foreach(Door door in doorArray)
        {
            door.lockDoor();
        }

        DisableRoomCollider();
    }

    public void UnlockDoors(float doorUnlockDelay)
    {
        StartCoroutine(UnlockDoorsRountine(doorUnlockDelay));
    }

    private IEnumerator UnlockDoorsRountine(float doorUnlockDelay)
    {
        if(doorUnlockDelay > 0)
        {
            yield return new WaitForSeconds(doorUnlockDelay);
        }
        Door[] doorArray = GetComponentsInChildren<Door>();

        foreach(Door door in doorArray)
        {
            door.UnlockDoor();
        }
        EnableRoomCollider();
    }
}
