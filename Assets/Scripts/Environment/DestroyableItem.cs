using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableItem : MonoBehaviour
{
    #region Header ����ֵ
    [Header("����ֵ")]
    #endregion

    #region Tooltip
    [Tooltip("������Ʒ��ʼ����ֵ")]
    #endregion
    [SerializeField] private int startingHealthAmount = 1;

    #region Header ��Ч
    [Header("��Ч")]
    #endregion

    #region Tooltip
    [Tooltip("������Ʒ������Ч")]
    #endregion
    [SerializeField] private SoundEffectSO soundEffect;

    private Animator animator;
    private BoxCollider2D boxCollider2D;
    private HealthEvent healthEvent;
    private Health health;
    private ReceiveContactDamage receiveContactDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        health.SetStartingHealth(startingHealthAmount);
        receiveContactDamage = GetComponent<ReceiveContactDamage>();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent,HealthEventArgs healthEventArgs)
    {
        if(healthEventArgs.healthAmount <= 0f)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        Destroy(boxCollider2D);

        if(soundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(soundEffect);
        }

        animator.SetBool(Settings.destroy, true);

        while(!animator.GetCurrentAnimatorStateInfo(0).IsName(Settings.stateDestroyed))
        {
            yield return null;
        }

        Destroy(animator);
        Destroy(receiveContactDamage);
        Destroy(health);
        Destroy(healthEvent);
        Destroy(this);
    }
}
