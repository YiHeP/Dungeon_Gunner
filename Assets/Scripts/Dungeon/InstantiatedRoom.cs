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
    [HideInInspector] public Bounds roomCollisionBounds;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        roomCollisionBounds = boxCollider2D.bounds;
    }

    public void Initialise(GameObject roomGameObject)
    {
        PopulateTilemapMemberVariable(roomGameObject);

        BlockOffUnuseDoorWays();

        AddDoorToRoom();

        DisableCollisionTilemapRenderer();
    }

    //�����Ƭ��ͼ��Ա����
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

    //����δʹ�õ��ŵ�
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
                    doorComponent.lockDoor();//���ͨ���������йؿ����ܴ�boss��
                }
            }
        }
    }

    //���� Collision Tilemap Renderer
    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;

    }
}
