using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

[DisallowMultipleComponent]
public class MoveItem : MonoBehaviour
{
    #region Header 音效
    [Space(10)]
    [Header("音效")]
    #endregion

    #region Tooltip
    [Tooltip("物品移动的音效")]
    #endregion
    [SerializeField] private SoundEffectSO soundEffect;

    [HideInInspector] public BoxCollider2D boxCollider2D;
    private new Rigidbody2D rigidbody2D;
    private InstantiatedRoom instantiatedRoom;
    private Vector3 previousPosition;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        instantiatedRoom = GetComponentInParent<InstantiatedRoom>();

        instantiatedRoom.moveableItemsList.Add(this);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        UpdateObstacles();
    }

    private void UpdateObstacles()
    {
        ConfineItemToRoomBounds();

        instantiatedRoom.UpdateMoveableObstacles();

        previousPosition = transform.position;

        if(Mathf.Abs(rigidbody2D.velocity.x) > 0.001f || Mathf.Abs(rigidbody2D.velocity.y) > 0.001f)
        {
            if(soundEffect != null && Time.frameCount % 10 == 0)
            {
                SoundEffectManager.Instance.PlaySoundEffect(soundEffect);
            }
        }
    }

    private void ConfineItemToRoomBounds()
    {
        Bounds itemBoumd = boxCollider2D.bounds;
        Bounds roomBound = instantiatedRoom.roomCollisionBounds;

        if(itemBoumd.min.x <= roomBound.min.x ||
            itemBoumd.min.y <=  roomBound.min.y ||
            itemBoumd.max.x >=  roomBound.max.x ||
            itemBoumd.max.y >= roomBound.max.y)
        {
            transform.position = previousPosition;
        }
    }

}
