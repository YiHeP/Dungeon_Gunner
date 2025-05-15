using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DungeonMap : SingletonMonobehaviour<DungeonMap>
{
    #region Haeder ��Ʒ����
    [Space(10)]
    [Header("��Ʒ����")]
    #endregion

    #region Tooltip
    [Tooltip("����С��ͼUI��Ʒ")]
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
