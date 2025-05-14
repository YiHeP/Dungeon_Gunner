using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Table : MonoBehaviour,IUseable
{
    #region Tooltip
    [Tooltip("填入物品的质量")]
    #endregion
    [SerializeField] private float itemMass;
    private BoxCollider2D boxCollider2D;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    private bool itemUse = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void UseItem()
    {
        if(!itemUse)
        {
            Bounds bounds = boxCollider2D.bounds;

            Vector3 closestPointToPlayer = bounds.ClosestPoint(GameManager.Instance.GetPlayer().GetPlayerPosition());

            if(closestPointToPlayer.x == bounds.max.x)
            {
                animator.SetBool(Settings.flipLeft, true);
            }
            else if(closestPointToPlayer.x == bounds.min.x)
            {
                animator.SetBool(Settings.flipRight,true);
            }
            else if(closestPointToPlayer.y == bounds.min.y)
            {
                animator.SetBool(Settings.flipUp, true);
            }
            else
            {
                animator.SetBool(Settings.flipDown, true);
            }

            gameObject.layer = LayerMask.NameToLayer("Environment");

            rigidbody2D.mass = itemMass;

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.tableFilp);

            itemUse = true;
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckPositiveValues(this,nameof(itemMass), itemMass,false);
    }
#endif
    #endregion
}
