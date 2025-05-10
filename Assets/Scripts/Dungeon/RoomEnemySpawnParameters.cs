using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    #region Tooltip
    [Tooltip("填入地牢等级")]
    #endregion
    public DungeonLevelSO dungeonLevel;

    #region Tooltip
    [Tooltip("敌人的最小生成量")]
    #endregion
    public int minTotalEnemiesToSpawn;

    #region Tooltip
    [Tooltip("敌人的最大生成量")]
    #endregion
    public int maxTotalEnemiesToSpawn;

    #region Tooltip
    [Tooltip("场上怪物的最小量")]
    #endregion
    public int minConcurrentEnemies;

    #region Tooltip
    [Tooltip("场上怪物的最大量")]
    #endregion
    public int maxConcurrentEnemies;

    #region Tooltip
    [Tooltip("最短生成间隔")]
    #endregion
    public int minSpawnInterval;

    #region Tooltip
    [Tooltip("最长生成间隔")]
    #endregion
    public int maxSpawnInterval;
}
