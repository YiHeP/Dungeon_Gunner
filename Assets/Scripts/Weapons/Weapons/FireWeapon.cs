using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(WeaponFireEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float fireRateCoolDownTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private WeaponFireEvent weaponFireEvent;
    private ReloadWeaponEvent reloadWeaponEvent;

    private void Awake()
    {
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        weaponFireEvent = GetComponent<WeaponFireEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>(); 
    }

    private void OnEnable()
    {
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
    }

    private void OnDisable()
    {
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;
    }

    private void Update()
    {
        fireRateCoolDownTimer -= Time.deltaTime;
    }

    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent,FireWeaponArgs fireWeaponArgs)
    {
        WeaponFire(fireWeaponArgs);
    }

    private void WeaponFire(FireWeaponArgs fireWeaponArgs)
    {
        if(fireWeaponArgs.fire)
        {
            if(IsWeaponReadyToFire())
            {
                FireAmmo(fireWeaponArgs.aimAngle, fireWeaponArgs.weaponAimAngle, fireWeaponArgs.weaponAimDirectionVector);

                RestartCoolDownTime();
            }
        }
    }

    private bool IsWeaponReadyToFire()
    {
        if(activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponsDetails.hasInfiniteAmmo)
        {
            return false;
        }
        if(activeWeapon.GetCurrentWeapon().isWeaponReloading)
        {
            return false;
        }
        if(fireRateCoolDownTimer > 0)
        {
            return false;
        }
        if(activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponsDetails.hasInfiniteClipCapacity)
        {
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);
            return false;
        }
        return true;
        
    }

    private void FireAmmo(float aimAngle, float weaponAngle, Vector3 weaponAimDirectionVector)
    {
        AmmoDetailSO currentAmmo = activeWeapon.GetCurrentAmmo();

         if(currentAmmo != null)
        {
            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0,currentAmmo.ammoPrefabArray.Length)];

            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

            ammo.InitialiseAmmo(currentAmmo,aimAngle,weaponAngle,ammoSpeed,weaponAimDirectionVector);

            if(!activeWeapon.GetCurrentWeapon().weaponsDetails.hasInfiniteClipCapacity)
            {
                activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
                activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
            }
            weaponFireEvent.CallWeaponFireEvent(activeWeapon.GetCurrentWeapon());
        }

    }

    private void RestartCoolDownTime()
    {
        fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponsDetails.weaponFireRate;
    }

}
