using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region Header ��Ʒ�ο�
    [Space(10)]
    [Header("��Ʒ�ο�")]
    #endregion

    #region Tooltip
    [Tooltip("����fadeScreen�е�messageText")]
    #endregion
    [SerializeField] private TextMeshProUGUI messageTextTMP;

    #region Tooltip
    [Tooltip("����fadeScreen�е�canvas groups")]
    #endregion
    [SerializeField] private CanvasGroup canvasGroup;

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

        StartCoroutine(Fade(0f,1f,0f,Color.black));
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
            case GameState.playingLevel:
                if(Input.GetKeyDown(KeyCode.M))
                {
                    DisPlayDungeonOverviewMap();
                }
                break;
            case GameState.dungeonOverviewMap:
                if(Input.GetKeyDown(KeyCode.M))
                {
                    DungeonMap.Instance.ClearDungeonOverViewMap();
                }
                break;
            case GameState.bossStage:
                if (Input.GetKeyDown(KeyCode.M))
                {
                    DisPlayDungeonOverviewMap();
                }
                break;
        }
    }

    private IEnumerator Fade(float startFadeAlpha,  float endFadeAlpha, float fadeSeconds,Color color)
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

        StartCoroutine(DisPlayDungeonLevelText());
    }

    private IEnumerator DisPlayDungeonLevelText()
    {
        StartCoroutine(Fade(0f,1f,0f,Color.black));

        GetPlayer().playerControl.DisablePlayerControl();

        string messageText = "�ؿ���" + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + dungeonLevelList
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

        yield return StartCoroutine(DisplayMessageRoutine("���Ĳ���" + GameResources.Instance.currentPlayer.playerName + "!�ܹ�������˴�\n\n"+
            "��������Ҫ�ҵ�������boss,ף����ˣ�",Color.white,5f));

        yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));
    }
    
    private IEnumerator LevelCompleted()
    {
        gameState = GameState.playingLevel;

        yield return new WaitForSeconds(2);

        yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));

        yield return StartCoroutine(DisplayMessageRoutine("�ɵú�" + GameResources.Instance.currentPlayer.playerName + "!\n\n" +
            "��ɹ����������������������ˣ�", Color.white, 5f));

        yield return StartCoroutine(DisplayMessageRoutine("�ռ�����ս��Ʒ������Return��\n\nǰ����һ�����", Color.white, 5f));

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

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        yield return StartCoroutine(DisplayMessageRoutine("��ϲ��" + GameResources.Instance.currentPlayer.playerName + "!\n\n" +
            "��ɹ��Ĵӵ����г��룡", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("��ĵ÷�Ϊ��"+ gameScore.ToString("###,##0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("����Return�����¿�ʼ��Ϸ", Color.white, 0f));

        gameState = GameState.restartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;

        GetPlayer().playerControl.DisablePlayerControl();

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }

        yield return StartCoroutine(DisplayMessageRoutine("�ǳ��ź�" + GameResources.Instance.currentPlayer.playerName + "!\n\n" +
            "���ڵ���֮�����䣡", Color.white, 3f));

        yield return StartCoroutine(DisplayMessageRoutine("��ĵ÷�Ϊ��" + gameScore.ToString("###,##0"), Color.white, 4f));

        yield return StartCoroutine(DisplayMessageRoutine("����Return�����¿�ʼ��Ϸ", Color.white, 0f));

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
        HelpUtilities.ValidateCheckNullValues(this,nameof(messageTextTMP), messageTextTMP);
        HelpUtilities.ValidateCheckNullValues(this,nameof(canvasGroup),canvasGroup);

    }

#endif

    #endregion

}
