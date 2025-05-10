using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    #region Tooltip
    [Tooltip("������εȼ�")]
    #endregion
    public DungeonLevelSO dungeonLevel;

    #region Tooltip
    [Tooltip("���˵���С������")]
    #endregion
    public int minTotalEnemiesToSpawn;

    #region Tooltip
    [Tooltip("���˵����������")]
    #endregion
    public int maxTotalEnemiesToSpawn;

    #region Tooltip
    [Tooltip("���Ϲ������С��")]
    #endregion
    public int minConcurrentEnemies;

    #region Tooltip
    [Tooltip("���Ϲ���������")]
    #endregion
    public int maxConcurrentEnemies;

    #region Tooltip
    [Tooltip("������ɼ��")]
    #endregion
    public int minSpawnInterval;

    #region Tooltip
    [Tooltip("����ɼ��")]
    #endregion
    public int maxSpawnInterval;
}
