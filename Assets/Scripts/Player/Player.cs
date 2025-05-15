using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


#region 所需组件
[RequireComponent(typeof(HealthEvent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(DestroyedEvent))]
[RequireComponent(typeof(Destroyed))]
[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFireEvent))]
[RequireComponent(typeof(MovementToPosition))]
[RequireComponent(typeof(FireWeapon))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadEvent))]
[RequireComponent(typeof(ReloadWeapon))]
[RequireComponent(typeof(Idle))]
[RequireComponent(typeof(IdleEvent))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DealContactDamage))]
[RequireComponent(typeof(ReceiveContactDamage))]

[DisallowMultipleComponent]
#endregion


public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public HealthEvent healthEvent;
    [HideInInspector] public Health health;
    [HideInInspector] public DestroyedEvent destroyedEvent;
    [HideInInspector] public IdleEvent idleEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public SetActiveWeaponEvent setActiveWeaponEvent;
    [HideInInspector] public ActiveWeapon activeWeapon;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;
    [HideInInspector] public FireWeaponEvent fireWeaponEvent;
    [HideInInspector] public WeaponFireEvent weaponFireEvent;
    [HideInInspector] public ReloadWeaponEvent reloadWeaponEvent;
    [HideInInspector] public WeaponReloadEvent weaponReloadEvent;
    [HideInInspector] public PlayerControl playerControl;

    public List<Weapon> weaponList = new List<Weapon>();
    public void Awake()
    {
        animator = GetComponent<Animator>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        destroyedEvent = GetComponent<DestroyedEvent>();
        idleEvent = GetComponent<IdleEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFireEvent = GetComponent<WeaponFireEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadEvent = GetComponent<WeaponReloadEvent>();
        playerControl = GetComponent<PlayerControl>();
    }

    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;

        CreatePlayerStaringWeapons();

        SetPlayerHealth();
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
        if(healthEventArgs.healthAmount <= 0 )
        {
            destroyedEvent.CallDestroyedEvent(true,0);
        }
    }

    private void CreatePlayerStaringWeapons()
    {
        weaponList.Clear();

        foreach(WeaponsDetailsSO weaponsDetails in  playerDetails.stasrtingWeaponList)
        {
            AddWeaponToPlayer(weaponsDetails);
        }
    }

    public Weapon AddWeaponToPlayer(WeaponsDetailsSO weaponsDetails)
    {
        Weapon weapon = new Weapon() { weaponsDetails = weaponsDetails,weaponReloadTimer = 0f,weaponClipRemainingAmmo = 
            weaponsDetails.weaponClipAmmoCapacity,weaponRemainingAmmo = weaponsDetails.weaponAmmoCapacity,isWeaponReloading = false};
        weaponList.Add(weapon);
        weapon.weaponListPosition = weaponList.Count;
        setActiveWeaponEvent.CallSetActiveWeaponEvent(weapon);
        return weapon;
    }

    public void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetails.PlayerHealthAmount);
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public bool IsWeaponHeldByPlayer(WeaponsDetailsSO weaponsDetails)
    {
        foreach(Weapon weapon in weaponList)
        {
            if(weapon.weaponsDetails == weaponsDetails)
                return true;
        }
        return false;
    }
}
