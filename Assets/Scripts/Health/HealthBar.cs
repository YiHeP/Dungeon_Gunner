using UnityEngine;

public class HealthBar : MonoBehaviour
{
    #region Header ��Ϸ��������
    [Space(10)]
    [Header("��Ϸ��������")]
    #endregion

    #region Tooltip
    [Tooltip("�����Ӷ���Ѫ��")]
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
