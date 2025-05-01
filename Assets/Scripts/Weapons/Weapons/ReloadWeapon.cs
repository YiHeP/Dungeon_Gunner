using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponReloadEvent))]
[RequireComponent(typeof(SetActiveWeaponEvent))]
[DisallowMultipleComponent]
public class ReloadWeapon : MonoBehaviour
{
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponReloadEvent weaponReloadEvent;
    private SetActiveWeaponEvent setActiveWeaponEvent;
    private Coroutine reloadWeaponCoroutine;

    private void Awake()
    {
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponReloadEvent = GetComponent<WeaponReloadEvent>();
        setActiveWeaponEvent = GetComponent<SetActiveWeaponEvent>();
    }

    public void OnEnable()
    {
        reloadWeaponEvent.OnReloadWeapon += ReloadWeaponEvent_OnReloadWeapon;

        setActiveWeaponEvent.OnSetActivrWeapon += SetActiveWeaponEvent_OnSetActivrWeapon;
    }

    public void OnDisable()
    {
        reloadWeaponEvent.OnReloadWeapon -= ReloadWeaponEvent_OnReloadWeapon;

        setActiveWeaponEvent.OnSetActivrWeapon -= SetActiveWeaponEvent_OnSetActivrWeapon;
    }

    public void ReloadWeaponEvent_OnReloadWeapon(ReloadWeaponEvent reloadWeaponEvent,ReloadWeaponArgs reloadWeaponArgs)
    {
        StartReloadWeapon(reloadWeaponArgs);
    }

    public void StartReloadWeapon(ReloadWeaponArgs reloadWeaponArgs)
    {
        if(reloadWeaponCoroutine != null)
        {
            StopCoroutine(reloadWeaponCoroutine);
        }
        reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(reloadWeaponArgs.weapon, reloadWeaponArgs.topUpAmmoPercent));
    }

    public IEnumerator ReloadWeaponRoutine(Weapon weapon, int  topUpAmmoPercent)
    {
        weapon.isWeaponReloading = true;
        while(weapon.weaponReloadTimer < weapon.weaponsDetails.weaponReloadTime)
        {
            weapon.weaponReloadTimer += Time.deltaTime;
            yield return null;
        }

        if(topUpAmmoPercent != 0)//从外部增加弹药
        {
            int ammoIncrease = Mathf.RoundToInt((weapon.weaponsDetails.weaponAmmoCapacity * topUpAmmoPercent) / 100f);

            int totalAmmo = weapon.weaponRemainingAmmo + ammoIncrease;

            if(totalAmmo > weapon.weaponsDetails.weaponAmmoCapacity)
            {
                weapon.weaponRemainingAmmo = weapon.weaponsDetails.weaponAmmoCapacity;
            }
            else
            {
                weapon.weaponRemainingAmmo = totalAmmo;
            }
        }

        if(weapon.weaponsDetails.hasInfiniteAmmo)//无限弹药
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponsDetails.weaponClipAmmoCapacity;
        }
        else if(weapon.weaponRemainingAmmo >= weapon.weaponsDetails.weaponClipAmmoCapacity)
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponsDetails.weaponClipAmmoCapacity;
        }
        else
        {
            weapon.weaponClipRemainingAmmo = weapon.weaponRemainingAmmo;
        }

        weapon.weaponReloadTimer = 0f;
        weapon.isWeaponReloading = false;
        weaponReloadEvent.CallWeaponReloadEvent(weapon);
    }

    public void SetActiveWeaponEvent_OnSetActivrWeapon(SetActiveWeaponEvent setActiveWeaponEvent,SetActivrWeaponArgs setActivrWeaponArgs)
    {
        if(setActivrWeaponArgs.weapon.isWeaponReloading)
        {
            if(reloadWeaponCoroutine != null)
            {
                StopCoroutine(reloadWeaponCoroutine);
            }
            reloadWeaponCoroutine = StartCoroutine(ReloadWeaponRoutine(setActivrWeaponArgs.weapon, 0));
        }
    }

}


