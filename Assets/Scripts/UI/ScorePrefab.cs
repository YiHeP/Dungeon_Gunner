using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePrefab : MonoBehaviour
{
    public TextMeshProUGUI rankTMP;
    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI levelTMP;
    public TextMeshProUGUI scoreTMP;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(rankTMP), rankTMP);
        HelpUtilities.ValidateCheckNullValues(this, nameof(nameTMP), nameTMP);
        HelpUtilities.ValidateCheckNullValues(this, nameof(levelTMP), levelTMP);
        HelpUtilities.ValidateCheckNullValues(this, nameof(scoreTMP), scoreTMP);
    }


#endif
    #endregion
}
