using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;
    private bool leftMouseDownPreviousFrame = false;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerRollCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private float playerRollCooldownTimer = 0f;
    private bool isPlayerMovementDisable = false;

    [HideInInspector] public bool isPlayerRolling = false;
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
            leftMouseDownPreviousFrame = false;
        }
    }

    private void SetPlayerAnimationSpeed()
    {
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimator;
    }

    private void Update()
    {
        if(isPlayerMovementDisable)
        {
            return;
        }

        if(isPlayerRolling)
        {
            return;
        }

        MoveMentInput();

        WeaponInput();

        UseItemInput();

        PlayerRollCooldownTimer();
    }

    private void MoveMentInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);

        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);
        if(horizontalMovement != 0 &&  verticalMovement != 0)//对角线行走
        {
            direction *= 0.7f;
        }

        if(direction != Vector2.zero)
        {
            if(!rightMouseButtonDown) //没有翻滚
            {
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            else if(playerRollCooldownTimer <=  0f)
            {
                PlayerRoll((Vector3)direction);//玩家翻滚
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
        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, aimDirection);
        SwitchWeaponInput();
        ReloadWeaponInput();
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

    private void FireWeaponInput(Vector3 weaponDirection,float weaponAngleDegrees,float playerAngleDegrees,AimDirection aimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            player.fireWeaponEvent.CallFireWeaponEvent(true, leftMouseDownPreviousFrame, aimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
            leftMouseDownPreviousFrame = true;
        }
        else
        {
            leftMouseDownPreviousFrame = false;
        }
    }

    private void SwitchWeaponInput()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            PreviousWeapon();
        }

        if(Input.mouseScrollDelta.y < 0)
        {
            NextWeapon();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeaponByIndex(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeaponByIndex(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeaponByIndex(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeaponByIndex(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWeaponByIndex(5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetWeaponByIndex(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetWeaponByIndex(7);
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetWeaponByIndex(8);
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetWeaponByIndex(9);
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetWeaponByIndex(0);
        }

        if(Input.GetKeyDown(KeyCode.Minus))
        {
            SetCurrentWeaponToFirstInTheList();
        }
    }

    private void PreviousWeapon()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 1)
        {
            currentWeaponIndex = player.weaponList.Count;
        }

        SetWeaponByIndex(currentWeaponIndex);
    }

    private void NextWeapon()
    {
        currentWeaponIndex++;

        if(currentWeaponIndex > player.weaponList.Count)
        {
            currentWeaponIndex = 1;
        }

        SetWeaponByIndex (currentWeaponIndex);
    }

    private void SetCurrentWeaponToFirstInTheList()
    {
        List<Weapon> tempWeaponList = new List<Weapon>();

        Weapon currentWeapon = player.weaponList[currentWeaponIndex - 1];

        currentWeapon.weaponListPosition = 1;

        int index = 2;

        tempWeaponList.Add(currentWeapon);

        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon == currentWeapon) continue;
            tempWeaponList.Add(weapon);
            weapon.weaponListPosition = index;
            index++;
        }

        player.weaponList = tempWeaponList;

        currentWeaponIndex = 1;

        SetWeaponByIndex(currentWeaponIndex);
    }

    private void PlayerRollCooldownTimer()
    {
        if(playerRollCooldownTimer >= 0f)
        {
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    private void ReloadWeaponInput()
    {
       Weapon currentWeapon = player.activeWeapon.GetCurrentWeapon();
        if (currentWeapon.isWeaponReloading) return;//正在装弹药中

        if (currentWeapon.weaponRemainingAmmo < currentWeapon.weaponsDetails.weaponClipAmmoCapacity && !currentWeapon.weaponsDetails.hasInfiniteAmmo)
            return;
        if (currentWeapon.weaponClipRemainingAmmo == currentWeapon.weaponsDetails.weaponClipAmmoCapacity)
            return;
        if(Input.GetKeyDown(KeyCode.R))
        {
            player.reloadWeaponEvent.CallReloadWeaponEvent(player.activeWeapon.GetCurrentWeapon(), 0);
        }

    }

    private void UseItemInput()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            float useItemRadius = 2f;

            Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(player.GetPlayerPosition(),useItemRadius);

            foreach(Collider2D collider in collider2DArray)
            {
                IUseable iUseable = collider.GetComponent<IUseable>();

                if(iUseable != null)
                {
                    iUseable.UseItem();
                }
            }
        }
    }

    public void EnablePlayerControl()
    {
        isPlayerMovementDisable = false;
    }

    public void DisablePlayerControl()
    {
        isPlayerMovementDisable = true;
        player.idleEvent.CallIdleEvent();
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
