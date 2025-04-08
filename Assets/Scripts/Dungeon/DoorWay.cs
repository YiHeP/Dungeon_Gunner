using UnityEngine;
[System.Serializable]
public class Doorway 
{
    public Vector2Int position;
    public Orientation orientation;
    public GameObject doorPrefab;
    #region Header
    [Header("开始复制的左上角位置")]
    #endregion
    public Vector2Int doorwayStartCopyPosition;
    #region Header
    [Header("要复制的门口瓦片的宽度")]
    #endregion
    public int doorwayCopyTileWidth;
    #region Header
    [Header("要复制的门口瓦片的高度")]
    #endregion
    public int doorwayCopyTileHeight;
    [HideInInspector]
    public bool isConnected = false;
    //是否不可用
    [HideInInspector]
    public bool isUnavailable = false;
}
