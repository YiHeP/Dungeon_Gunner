using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private InstantiatedRoom bossRoom;

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

        StaticEventHandler.OnRoomEnemiesDefeated += StaticEventHandler_OnRoomEnemiesDefeated;

        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;

        StaticEventHandler.OnPointScored -= StaticEventHandler_OnPointScored;

        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;

        StaticEventHandler.OnRoomEnemiesDefeated -= StaticEventHandler_OnRoomEnemiesDefeated;

        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
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

    private void StaticEventHandler_OnRoomEnemiesDefeated(RoomEnemiesDefeatedArgs roomEnemiesDefeatedArgs)
    {
        RoomEnemiesDefeated();
    }

    private void Player_OnDestroyed(DestroyedEvent destroyedEvent,DestroyedEventArgs destroyedEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.gameLost;
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
                RoomEnemiesDefeated();
                break;
            case GameState.levelCompleted:
                StartCoroutine(LevelCompleted());
                break;
            case GameState.gameWon:
                if(previousGameState != GameState.gameWon)
                {
                    StartCoroutine(GameWon());
                }
                break;
            case GameState.gameLost:
                if(previousGameState != GameState.gameLost)
                {
                    StopAllCoroutines();
                    StartCoroutine(GameLost());
                }
                break;
            case GameState.restartGame:
                RestartGame();
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

    private void PlayerDungeonLevel(int dungeonLevelListIndex)//�����µ���
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

    private void RoomEnemiesDefeated()
    {
        bool isDungeonClearOfRegularEnemies = true;
        bossRoom = null;

        foreach(KeyValuePair<string,Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            if(keyValuePair.Value.roomNodeType.isBossRoom)
            {
                bossRoom = keyValuePair.Value.instantiatedRoom;
                continue;
            }

            if(!keyValuePair.Value.isCleanedOfEnemies)
            {
                isDungeonClearOfRegularEnemies = false;
                break;
            }
        }

        if((isDungeonClearOfRegularEnemies && bossRoom == null) || (isDungeonClearOfRegularEnemies && bossRoom.room.isCleanedOfEnemies))
        {
            if(currentDungeonLevelListIndex < dungeonLevelList.Count - 1)
            {
                gameState = GameState.levelCompleted;
            }
            else
            {
                gameState = GameState.gameWon;
            }
        }
        else if(isDungeonClearOfRegularEnemies)
        {
            gameState = GameState.bossStage;

            StartCoroutine(BossStage());
        }
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

    private IEnumerator BossStage()
    {
        bossRoom.gameObject.SetActive(true);
        bossRoom.UnlockDoors(0);
        yield return new WaitForSeconds(2);

        Debug.Log("����boss");
    }
    
    private IEnumerator LevelCompleted()
    {
        gameState = GameState.playingLevel;

        yield return new WaitForSeconds(2);

        Debug.Log("�ؿ�����");

        while(!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
        yield return null;
        currentDungeonLevelListIndex++;
        PlayerDungeonLevel(currentDungeonLevelListIndex);
    }

    private IEnumerator GameWon()
    {
        previousGameState = GameState.gameWon;

        Debug.Log("��Ϸʤ��");

        yield return new WaitForSeconds(10);

        gameState = GameState.restartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        Debug.Log("��Ϸʧ��");

        yield return new WaitForSeconds(10);

        gameState = GameState.restartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainGameScene");
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
