using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DungeonMap : SingletonMonobehaviour<DungeonMap>
{
    #region Haeder 物品引用
    [Space(10)]
    [Header("物品引用")]
    #endregion

    #region Tooltip
    [Tooltip("填入小地图UI物品")]
    #endregion
    [SerializeField] private GameObject minimapUI;
    private Camera dungeonMapCamera;
    private Camera cameraMain;

    private void Start()
    {
        cameraMain = Camera.main;

        Transform playerTranform = GameManager.Instance.GetPlayer().transform;

        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        cinemachineVirtualCamera.Follow = playerTranform;

        dungeonMapCamera = GetComponentInChildren<Camera>();

        dungeonMapCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && GameManager.Instance.gameState == GameState.dungeonOverviewMap)
        {
            GetRoomClick();
        }
    }

    private void GetRoomClick()
    {
        Vector3 worldPositoin = dungeonMapCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPositoin = new Vector3(worldPositoin.x, worldPositoin.y, 0);

        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(new Vector2(worldPositoin.x, worldPositoin.y), 1f);

        foreach(Collider2D collider in collider2DArray)
        {
            if(collider.GetComponent<InstantiatedRoom>() != null)
            {
                InstantiatedRoom instantiatedRoom = collider.GetComponent<InstantiatedRoom>();

                if(instantiatedRoom.room.isCleanedOfEnemies && instantiatedRoom.room.isPreviouslyVisited)
                {
                    StartCoroutine(MovePlayerToRoom(worldPositoin, instantiatedRoom.room));
                }
            }
        }
     }

    private IEnumerator MovePlayerToRoom(Vector3 worldPositoin,Room room)
    {
        StaticEventHandler.CallRoomChangedEvent(room);

        yield return StartCoroutine(GameManager.Instance.Fade(0f, 1f, 0f, Color.black));

        ClearDungeonOverViewMap();

        GameManager.Instance.GetPlayer().playerControl.DisablePlayerControl();

        Vector3 spawnPosition = HelpUtilities.GetSpawnPositionNearestToPlayer(worldPositoin);

        GameManager.Instance.GetPlayer().transform.position = spawnPosition;

        yield return StartCoroutine(GameManager.Instance.Fade(1f,0f,1f,Color.black));

        GameManager.Instance.GetPlayer().playerControl.EnablePlayerControl();
    }

    public void DisPlayDungeonOverViewMap()
    {
        GameManager.Instance.previousGameState = GameManager.Instance.gameState;

        GameManager.Instance.gameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.DisablePlayerControl();

        cameraMain.gameObject.SetActive(false);
        dungeonMapCamera?.gameObject.SetActive(true);

        ActivateRoomsForDisPlay();

        minimapUI.SetActive(false);
    }

    public void ClearDungeonOverViewMap()
    {
        GameManager.Instance.gameState = GameManager.Instance.previousGameState;
        GameManager.Instance.previousGameState= GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.EnablePlayerControl();
        cameraMain.gameObject.SetActive(true);
        dungeonMapCamera?.gameObject.SetActive(false);

        minimapUI?.SetActive(true);
    }

    private void ActivateRoomsForDisPlay()
    {
        foreach(KeyValuePair<string,Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            room.instantiatedRoom.gameObject.SetActive(true);
        }
    }
}
