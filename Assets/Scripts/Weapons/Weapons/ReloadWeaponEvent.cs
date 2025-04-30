using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ReloadWeaponEvent : MonoBehaviour
{
    public event Action<ReloadWeaponEvent,ReloadWeaponArgs> OnReloadWeapon;

    public void CallReloadWeaponEvent(Weapon weapon, int topUpAmmoPercent)
    {
        OnReloadWeapon?.Invoke(this, new ReloadWeaponArgs()
        {
            weapon = weapon,
            topUpAmmoPercent = topUpAmmoPercent
        });
    }

}

public class ReloadWeaponArgs:EventArgs
{
    public Weapon weapon;
    public int topUpAmmoPercent;//充值弹药百分比
}
