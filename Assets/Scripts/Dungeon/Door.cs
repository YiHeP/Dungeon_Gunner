using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Door : MonoBehaviour
{
    #region 物体引用
    [Space(10)]
    [Header("物体引用")]
    #endregion

    #region Tooltip
    [Tooltip("门的碰撞器")]
    #endregion
    [SerializeField] private BoxCollider2D doorCollider;

    [HideInInspector] public bool isBossRoomDoor = false;

    private BoxCollider2D doorTrigger;
    private bool isOpen = false;
    private bool previouslyOpened = false;
    private Animator animator;

    private void Awake()
    {
        doorCollider.enabled = false;

        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Settings.playerTag ||  collision.tag == Settings.playerWeaponTag)
        {
            openDoor();
        }
    }

    private void OnEnable()
    {
        animator.SetBool(Settings.open, isOpen);
    }

    public void openDoor()
    {
        if(!isOpen)
        {
            isOpen = true;
            previouslyOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;

            animator.SetBool(Settings.open,true);

            SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.doorOpenCloseSoundEffect);
        }
    }  
    
    public void lockDoor()
    {
        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        animator.SetBool(Settings.open,false);
    }

    public void unLockDoor()
    {
        doorCollider.enabled = false;
        doorTrigger.enabled = true;
        if(previouslyOpened)
        {
            isOpen = false;
            openDoor();
        }
    }

    #region
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(doorCollider), doorCollider);
    }
#endif
    #endregion
}
