using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Settings
{
    #region ��λ
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region ���ι�ģ
    public const int maxDungeonRebuildAttemptsForRoomNodeGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion

    #region ��������
    public const float fadeInTime = 0.5f;
    public const int MaxChildCorridors = 3;
    public const float doorUnlockDelay = 1f;
    #endregion

    #region ��������
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

    public static int flipUp = Animator.StringToHash("flipUp");
    public static int flipDown = Animator.StringToHash("flipDown");
    public static int flipLeft = Animator.StringToHash("flipLeft");
    public static int flipRight = Animator.StringToHash("flipRight");
    public static int use = Animator.StringToHash("use"); 

    public static float baseSpeedForPlayerAnimator = 8f;

    public static float baseSpeedForEnemyAnimator = 3f;

    public static int open = Animator.StringToHash("open");

    public static int destroy = Animator.StringToHash("destroy");
    public static String stateDestroyed = "Destroyed";
    #endregion

    #region ��Ϸ�����ǩ
    public const string playerTag = "Player";
    public const string playerWeaponTag = "playerWeapon";
    #endregion

    #region ��Ƶ
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion

    #region �������
    public const float useAimAngleDistance = 3.5f;
    #endregion

    #region a���㷨����
    public const int defaultAStarMovementPenalty = 40;
    public const int perferredPathAStarMovementPenalty = 1;
    public const int targetFrameRateToSpreadPathfindingOver = 60;
    public const float playerMoveDistanceToRebuildPath = 3f;//����ƶ����پ����������¹���һ���µ�·
    public const float enemyPathRebuildCooldown = 2f;//�ؽ���·����ȴʱ��
    #endregion

    #region UI ����
    public const float uiAmmoIconSpacing = 4f;
    public const float uiHeartSpacing = 16f; 
    #endregion

    #region Header ���˲���
    public const int defaultEnemyHealth = 20;
    #endregion

    #region �Ӵ��˺�����
    public const float contactDamageCollisionResetDelay = 0.5f;
    #endregion
}
