using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class DealContactDamage : MonoBehaviour
{
    #region Header ����˺�
    [Space(10)]
    [Header("����˺�")]
    #endregion

    #region Tooltip
    [Tooltip("��ɵĽӴ��˺�")]
    #endregion
    [SerializeField] private int contactDamageAmount;

    #region Tooltip
    [Tooltip("�ܱ�����˺���ͼ��")]
    #endregion
    [SerializeField] private LayerMask layerMask;

    private bool isColliding = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;
        //Debug.Log("������" + gameObject.name);
        ContactDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliding) return;

        ContactDamage(collision);
    }

    private void ContactDamage(Collider2D collision)
    {
        int collisionObjectLayerMask = (1<<collision.gameObject.layer);

        if ((layerMask.value & collisionObjectLayerMask) == 0)
            return;
        ReceiveContactDamage receiveContactDamage = collision.gameObject.GetComponent<ReceiveContactDamage>();

        if(receiveContactDamage != null)
        {
            isColliding = true;

            Invoke("ResetContactCollision", Settings.contactDamageCollisionResetDelay);

            receiveContactDamage.TakeContactDamage(contactDamageAmount);
        }
    }

    private void ResetContactCollision()
    {
        isColliding = false;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckPositiveValues(this,nameof(contactDamageAmount),contactDamageAmount,false);
    }
#endif
    #endregion
}
