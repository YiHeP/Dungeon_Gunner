using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region 单位
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region 地牢规模
    public const int maxDungeonRebuildAttemptsForRoomNodeGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion

    #region 房间设置
    public const int MaxChildCorridors = 3;
    #endregion

    #region 动画参数
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int rollDown = Animator.StringToHash("rollDown");
    #endregion
}
