using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class ReceiveContactDamage : MonoBehaviour
{
    #region Header
    [Space(10)]
    [Header("½üÕ½ÉËº¦Á¿")]
    #endregion
    [SerializeField] private int contactDamageAmount;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    public void TakeContactDamage(int damageAmount = 0)
    {
        if(contactDamageAmount > 0)
        {
            damageAmount = contactDamageAmount;
        }

        health.TakeDamage(damageAmount);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckPositiveValues(this, nameof(contactDamageAmount), contactDamageAmount,true);
    }
#endif
    #endregion
}
