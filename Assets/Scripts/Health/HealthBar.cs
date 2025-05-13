using UnityEngine;

public class HealthBar : MonoBehaviour
{
    #region Header 游戏对象引用
    [Space(10)]
    [Header("游戏对象引用")]
    #endregion

    #region Tooltip
    [Tooltip("填入子对象血条")]
    #endregion 
    [SerializeField] private GameObject healthBar;

    public void EnableHealthBar()
    {
        gameObject.SetActive(true);
    }

    public void DisableHealthBar()
    {
        gameObject.SetActive(false);
    }

    public void SetHealthBarValue(float healthPercent)
    {
        healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
    }
}
