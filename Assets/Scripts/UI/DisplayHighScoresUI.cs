using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHighScoresUI : MonoBehaviour
{
    #region Header 物品引用
    [Space(10)]
    [Header("物品引用")]
    #endregion

    #region Tooltip
    [Tooltip("填充子游戏对象Transform组件")]
    #endregion Tooltip
    [SerializeField] private Transform contentAnchorTransform;

    private void Start()
    {
        DisplayScores();
    }

    private void DisplayScores()
    {
        HighScore highScores = HighScoreManager.Instance.GetHighScores();
        GameObject scoreGameobject;

        int rank = 0;
        foreach (Score score in highScores.scoreList)
        {
            rank++;

            scoreGameobject = Instantiate(GameResources.Instance.scorePrefab, contentAnchorTransform);

            ScorePrefab scorePrefab = scoreGameobject.GetComponent<ScorePrefab>();

            scorePrefab.rankTMP.text = rank.ToString();
            scorePrefab.nameTMP.text = score.playerName;
            scorePrefab.levelTMP.text = score.levelDescription;
            scorePrefab.scoreTMP.text = score.playerScore.ToString("###,###0");
        }
    }
}
