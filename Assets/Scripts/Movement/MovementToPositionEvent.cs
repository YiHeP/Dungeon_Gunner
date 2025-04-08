using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionArgs> OnMovementToPosition;

    public void CallMovementToPositionEvent(Vector3 movePositon, Vector3 currencePosition, float moveSpeed, Vector2 moveDirection, bool isRolling
        = false)
    {
        OnMovementToPosition?.Invoke(this, new MovementToPositionArgs()
        {
            moveDirection = moveDirection,
            currencePosition = currencePosition,
            movePositon = movePositon,
            moveSpeed = moveSpeed,
            isRolling = isRolling
        });
    }
}

public class MovementToPositionArgs:EventArgs
{
    public Vector3 movePositon;
    public Vector3 currencePosition;
    public float moveSpeed;
    public Vector2 moveDirection;
    public bool isRolling;
}