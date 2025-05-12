using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region ���ιؿ�
    [Space(10)]
    [Header("���ιؿ�")]
    #endregion

    #region Tooltip
    [Tooltip("�������ĵ��ιؿ�")]
    #endregion

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("���뵱ǰ�Ĺؿ���ţ�0Ϊ��һ��")]
    #endregion

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long gameScore;
    private int scoreMultiplier;

    protected override void Awake()
    {
        base.Awake();
        playerDetails = GameResources.Instance.currentPlayerSO.playerDetails;

        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);
        player = playerGameObject.GetComponent<Player>();
        player.Initialize(playerDetails);//���������Ϣ
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnPointScored += StaticEventHandler_OnPointScored;

        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnPointScored -= StaticEventHandler_OnPointScored;

        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        SetCurrentRoom(roomChangedEventArgs.room);
    }

    private void StaticEventHandler_OnPointScored(PointScoredArgs pointScoredArgs)
    {
        gameScore += pointScoredArgs.points * scoreMultiplier;

        StaticEventHandler.CallScoreChangedEvent(gameScore,scoreMultiplier);
    }

    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {
        if (multiplierArgs.multiplier)
            scoreMultiplier++;
        else
            scoreMultiplier--;
        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30);

        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void Start()
    {
        gameState = GameState.gameStarted;
        previousGameState = GameState.gameStarted;

        gameScore = 0;
        scoreMultiplier = 1;
    }

    private void Update()
    {
        HandleGameState();

        //����ʹ��
        //if(Input.GetKeyUp(KeyCode.P))
        //{
        //    gameState = GameState.gameStarted;
        //}


    }

    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.gameStarted:
                PlayerDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;
                break;
        }
    }

    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom; 
        currentRoom = room;
    }

    public Player GetPlayer() 
    { 
        return player; 
    }

    private void PlayerDungeonLevel(int dungeonLevelListIndex)
    {
        bool dungeonBuiltSuccessful = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);
        if(!dungeonBuiltSuccessful)
        {
            Debug.Log("��������ʧ��");
        }
        StaticEventHandler.CallRoomChangedEvent(currentRoom);
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom
            .lowerBounds.y + currentRoom.upperBounds.y), 0f);
        player.gameObject.transform.position = HelpUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public Sprite GetMiniMapPlayer()
    {
        return playerDetails.PlayerMiniMapIcon;
    }

    public DungeonLevelSO GetCurrentDungeonLevel()
    {
        return dungeonLevelList[currentDungeonLevelListIndex];
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }

#endif

    #endregion

}
