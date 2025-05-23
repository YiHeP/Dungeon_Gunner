using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{

    #region Header 物品引用
    [Space(10)]
    [Header("物品引用")]
    #endregion

    #region Tooltip
    [Tooltip("填入暂停菜单的物品")]
    #endregion
    [SerializeField] private GameObject psuseMenu;

    #region Tooltip
    [Tooltip("填入fadeScreen中的messageText")]
    #endregion
    [SerializeField] private TextMeshProUGUI messageTextTMP;

    #region Tooltip
    [Tooltip("填入fadeScreen中的canvas groups")]
    #endregion
    [SerializeField] private CanvasGroup canvasGroup;

    #region 地牢关卡
    [Space(10)]
    [Header("地牢关卡")]
    #endregion

    #region Tooltip
    [Tooltip("添加所需的地牢关卡")]
    #endregion

    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("输入当前的关卡序号，0为第一关")]
    #endregion

    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    public long gameScore;
    private int scoreMultiplier;
    private InstantiatedRoom bossRoom;
    private bool isFading = false;

    protected override void Awake()
    {
        base.Awake();
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        InstantiatePlayer();
    }

    private void InstantiatePlayer()
    {
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);
        player = playerGameObject.GetComponent<Player>();
        player.Initialize(playerDetails);//配置玩家信息
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

        StartCoroutine(Fade(0f,1f,0f,Color.black));
    }

    private void Update()
    {
        HandleGameState();

        //测试使用
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
            case GameState.playingLevel:
                if(Input.GetKeyDown(KeyCode.M))
                {
                    DisPlayDungeonOverviewMap();
                }

                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
            case GameState.dungeonOverviewMap:
                if(Input.GetKeyDown(KeyCode.M))
                {
                    DungeonMap.Instance.ClearDungeonOverViewMap();
                }
                break;
            case GameState.engagingEnemies:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
            case GameState.bossStage:
                if (Input.GetKeyDown(KeyCode.M))
                {
                    DisPlayDungeonOverviewMap();
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
            case GameState.engagingBoss:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
            case GameState.gamePaused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
        }
    }

    public IEnumerator Fade(float startFadeAlpha,  float endFadeAlpha, float fadeSeconds,Color color)
    {
        isFading = true;

        Image image = canvasGroup.GetComponent<Image>();
        image.color = color;

        float time = 0;
        while(time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, endFadeAlpha, time/fadeSeconds);
            yield return null;
        }
        isFading= false;
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

    private void PlayerDungeonLevel(int dungeonLevelListIndex)//构建新地牢
    {
        bool dungeonBuiltSuccessful = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);

        if(!dungeonBuiltSuccessful)
        {
            Debug.Log("构建地牢失败");
        }
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom
            .lowerBounds.y + currentRoom.upperBounds.y), 0f);

        player.gameObject.transform.position = HelpUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

        StartCoroutine(DisPlayDungeonLevelText());
    }

    private IEnumerator DisPlayDungeonLevelText()
    {
        StartCoroutine(Fade(0f,1f,0f,Color.black));

        GetPlayer().playerControl.DisablePlayerControl();

        string messageText = "关卡：" + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + dungeonLevelList
            [currentDungeonLevelListIndex].levelName.ToString();

        yield return StartCoroutine(DisplayMessageRoutine(messageText,Color.white,2f));

        GetPlayer().playerControl.EnablePlayerControl();

        yield return StartCoroutine(Fade(1f,0f,2f,Color.black));

    }

    private IEnumerator DisplayMessageRoutine(string message, Color color,float displayTime)
    {
        messageTextTMP.SetText(message);
        messageTextTMP.color = color;

        if(displayTime > 0f)
        {
            float time = displayTime;

            while(time > 0f && !Input.GetKeyDown(KeyCode.Return))
            {
                time -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while(!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }
        yield return null;

        messageTextTMP.SetText("");
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

    private void DisPlayDungeonOverviewMap()
    {
        if (isFading) return;
        DungeonMap.Instance.DisPlayDungeonOverViewMap();
    }

    public void PauseGameMenu()
    {
        if(gameState != GameState.gamePaused)
        {
            psuseMenu.SetActive(true); 
            GetPlayer().playerControl.DisablePlayerControl();

            previousGameState = gameState;
            gameState = GameState.gamePaused;
        }
        else if(gameState == GameState.gamePaused)
        {
            psuseMenu.SetActive(false);
            GetPlayer().playerControl.EnablePlayerControl();

            gameState = previousGameState;
            previousGameState = GameState.gamePaused;
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

        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine("做的不错" + GameResources.Instance.currentPlayer.playerName + "!能够存活至此处\n\n"+
            "现在你需要找到并击败boss,祝你好运！",Color.white,5f));

        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));
    }
    
    private IEnumerator LevelCompleted()
    {
        gameState = GameState.playingLevel;

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine("干得好" + GameResources.Instance.currentPlayer.playerName + "!\n\n" +
            "你成功的在这层地牢中生存下来了！", Color.white, 5f));

        yield return StartCoroutine(DisplayMessageRoutine("收集所有战利品，按下Return键\n\n前往下一层地牢", Color.white, 5f));

        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        while (!Input.GetKeyDown(KeyCode.Return))
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

        GetPlayer().playerControl.DisablePlayerControl();

        int rank = HighScoreManager.Instance.GetRank(gameScore);

        string rankText;

        if(rank > 0 && rank <= Settings.numberOfHighScoresToSave)
        {
            rankText = "你的得分排名为：" + rank.ToString("#0") + "名，位列前" + Settings.numberOfHighScoresToSave.ToString("#0")+"名之中";

            string name = GameResources.Instance.currentPlayer.playerName;

            if(name == "")
            {
                name = playerDetails.playerCharacterName;
            }

            HighScoreManager.Instance.AddScore(new Score()
            {
                playerName = name,
                levelDescription = "Level" + (currentDungeonLevelListIndex + 1).ToString() + " - " + GetCurrentDungeonLevel().levelName,
                playerScore = gameScore
            },rank);
        }
        else
        {
            rankText = "你的得分低于前" + Settings.numberOfHighScoresToSave.ToString("#0") + "名之中";
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        yield return StartCoroutine(DisplayMessageRoutine("恭喜你" + GameResources.Instance.currentPlayer.playerName + "!\n\n" +
            "你成功的从地牢中撤离！", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("你的得分为："+ gameScore.ToString("###,##0") + "\n\n" + rankText, Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("按下Return键重新开始游戏", Color.white, 0f));

        gameState = GameState.restartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        GetPlayer().playerControl.DisablePlayerControl();

        int rank = HighScoreManager.Instance.GetRank(gameScore);

        string rankText;

        if (rank > 0 && rank <= Settings.numberOfHighScoresToSave)
        {
            rankText = "你的得分排名为：" + rank.ToString("#0") + "名，位列前" + Settings.numberOfHighScoresToSave.ToString("#0") + "名之中";

            string name = GameResources.Instance.currentPlayer.playerName;

            if (name == "")
            {
                name = playerDetails.playerCharacterName;
            }

            HighScoreManager.Instance.AddScore(new Score()
            {
                playerName = name,
                levelDescription = "Level" + (currentDungeonLevelListIndex + 1).ToString() + " - " + GetCurrentDungeonLevel().levelName,
                playerScore = gameScore
            }, rank);
        }
        else
        {
            rankText = "你的得分低于前" + Settings.numberOfHighScoresToSave.ToString("#0") + "名之中";
        }

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        yield return StartCoroutine(DisplayMessageRoutine("非常遗憾" + GameResources.Instance.currentPlayer.playerName + "!\n\n" +
            "你于地牢之中陨落！", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("你的得分为：" + gameScore.ToString("###,##0") + "\n\n" + rankText, Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("按下Return键重新开始游戏", Color.white, 0f));

        gameState = GameState.restartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelpUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
        HelpUtilities.ValidateCheckNullValues(this,nameof(messageTextTMP), messageTextTMP);
        HelpUtilities.ValidateCheckNullValues(this,nameof(canvasGroup),canvasGroup);
        HelpUtilities.ValidateCheckNullValues(this, nameof(psuseMenu), psuseMenu);
    }

#endif

    #endregion

}
