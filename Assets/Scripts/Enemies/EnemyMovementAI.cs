using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    #region Tooltip
    [Tooltip("����ƶ�ϸ��")]
    #endregion
    [SerializeField] private MovementDetailsSO movementDetails;
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>();
    private Vector3 playerReferencePosition;
    private Coroutine moveEnemyRoutine;
    private float currentEnemyPathRebuildCooldown;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public float moveSpeed;
    private bool chasePlayer = false;
    [HideInInspector] public int updateFrameNumber = 1;
    private List<Vector2Int> surroundingPositionList = new List<Vector2Int>();

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
    }

    private void Update()
    {
        MoveEnemy();
    }

    private void MoveEnemy()
    {
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        if(!chasePlayer && Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.enemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }
        
        if(!chasePlayer)
        {
            return;
        }

        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != updateFrameNumber)
            return;

        if(currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePosition,GameManager.Instance.GetPlayer().GetPlayerPosition()) > 
           Settings.playerMoveDistanceToRebuildPath))
        {
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;

            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            CreatePath();

            if(movementSteps != null)
            {
                if(moveEnemyRoutine != null)
                {
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine); 
                }
                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));
            }
        }
    }

    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);

        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom);

        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        if(movementSteps != null)
        {
            movementSteps.Pop();
        }
        else
        {
            enemy.idleEvent.CallIdleEvent();
        }
    }

    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }

    private Vector3Int GetNearestNonObstaclePlayerPosition(Room room)
    {
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

        Vector3Int playerCellPosition = room.instantiatedRoom.grid.WorldToCell(playerPosition);

        Vector2Int adjustedPlayerCellPosition = new Vector2Int(playerCellPosition.x - room.templateLowerBounds.x,playerCellPosition.y - 
            room.templateLowerBounds.y);

        int obstacle = Mathf.Min(room.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x,adjustedPlayerCellPosition.y],
            room.instantiatedRoom.aStartItemObstacles[adjustedPlayerCellPosition.x,adjustedPlayerCellPosition.y]);

        if(obstacle != 0)
        {
            return playerCellPosition;
        }
        else//���������ϰ����ϻᵼ���޷�����·����������ҪѰ��һ����������ϰ��ؿ���Ϊ�յ�
        {
            surroundingPositionList.Clear();

            for(int i = -1;i<=1;i++)
            {
                for(int j = -1;j<=1;j++)
                {
                    if (j == 0 && i == 0) continue;

                    surroundingPositionList.Add(new Vector2Int(i, j));
                }
            }

            for(int l = 0;l < 8;l++)
            {
                int index = Random.Range(0, surroundingPositionList.Count);
                try
                {
                    obstacle = Mathf.Min(room.instantiatedRoom.aStartItemObstacles[adjustedPlayerCellPosition.x + surroundingPositionList[index].x,
                        adjustedPlayerCellPosition.y + surroundingPositionList[index].y], 
                        room.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x+ surroundingPositionList[index].x,
                        adjustedPlayerCellPosition.y + surroundingPositionList[index].y]);
                    if(obstacle != 0)
                    {
                        return new Vector3Int(playerCellPosition.x + surroundingPositionList[index].x, playerCellPosition.y +
                            surroundingPositionList[index].y,0);
                    }
                }
                catch
                {

                }
                surroundingPositionList.RemoveAt(index);
            }
            #region ע�ʹ���
            //for(int i = -1; i <= 1; i++)
            //{
            //    for(int j = -1; j <= 1; j++)
            //    {
            //        if (i == 0 && j == 0) continue;
            //        try
            //        {
            //            obstacle = room.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPosition.x + i, adjustedPlayerCellPosition.y + j];
            //            if( obstacle != 0 )
            //            {
            //                return new Vector3Int(playerCellPosition.x + i,playerCellPosition.y + j,0);
            //            }
            //        }
            //        catch
            //        {
            //            continue;
            //        }
            //    }
            //}
            #endregion
            return (Vector3Int)room.spawnPositionArray[Random.Range(0, room.spawnPositionArray.Length)];
        }
    }

    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while(movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            while(Vector3.Distance(nextPosition,transform.position) > 0.2f)
            {
                enemy.movementToPositionEvent.CallMovementToPositionEvent(nextPosition,transform.position,moveSpeed,(nextPosition - 
                    transform.position).normalized);
                yield return waitForFixedUpdate;
            }
            yield return waitForFixedUpdate;
        }
        enemy.idleEvent.CallIdleEvent();
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion

}
