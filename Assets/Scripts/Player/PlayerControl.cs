using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private bool isPlayerRolling = false;
    private float playerRollCooldownTimer = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        SetStartingWeapon();

        SetPlayerAnimationSpeed();
    }

    private void SetStartingWeapon()
    {
        int index = 1;
        foreach(Weapon weapon in player.weaponList)
        {
            if(weapon.weaponsDetails == player.playerDetails.staringWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }

    private void SetWeaponByIndex(int index)
    {
        if(index - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = index;
            player.setActiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[index - 1]);
        }
    }

    private void SetPlayerAnimationSpeed()
    {
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimator;
    }

    private void Update()
    {
        if(isPlayerRolling)
        {
            return;
        }

        MoveMentInput();

        WeaponInput();

        PlayerRollCooldownTimer();
    }

    private void MoveMentInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);

        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);
        if(horizontalMovement != 0 &&  verticalMovement != 0)
        {
            direction *= 0.7f;
        }

        if(direction != Vector2.zero)
        {
            if(!rightMouseButtonDown) 
            {
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            else if(playerRollCooldownTimer <=  0f)
            {
                PlayerRoll((Vector3)direction);
            }
        }
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    private void PlayerRoll(Vector3 direction)
    {
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        float minDistance = 0.2f;

        isPlayerRolling = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;

        while(Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed,
                direction, isPlayerRolling);

            yield return waitForFixedUpdate;
        }

        isPlayerRolling = false;

        playerRollCooldownTimer = movementDetails.rollCoolDownTime;

        player.transform.position = targetPosition;
    }

    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection aimDirection;
        AimWeaponInput(out weaponDirection,out weaponAngleDegrees,out playerAngleDegrees,out aimDirection);
    }

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out 
        AimDirection aimDirection)
    {
        Vector3 mouseWorldPosition = HelpUtilities.GetMouseWorldPosition();

        weaponDirection = (mouseWorldPosition - player.activeWeapon.GetShootPosition());

        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        weaponAngleDegrees = HelpUtilities.GetAngleFromVector(weaponDirection);

        playerAngleDegrees = HelpUtilities.GetAngleFromVector(playerDirection);

        aimDirection = HelpUtilities.GetAimDirection(playerAngleDegrees);

        player.aimWeaponEvent.CallAimWeaponEvent(aimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    private void PlayerRollCooldownTimer()
    {
        if(playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopPlayerRollRoutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StopPlayerRollRoutine();
    }

    private void StopPlayerRollRoutine()
    {
        if(playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);

            isPlayerRolling = false;
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelpUtilities.ValidateCheckNullValues(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion

}
