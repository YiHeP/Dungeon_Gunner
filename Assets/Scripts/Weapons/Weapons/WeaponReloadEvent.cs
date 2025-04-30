using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponReloadEvent : MonoBehaviour
{
    public Action<WeaponReloadEvent,WeaponReloadArgs> OnWeaponReloaded;

    public void CallWeaponReloadEvent(Weapon weapon)
    {
        OnWeaponReloaded?.Invoke(this, new WeaponReloadArgs() { weapon = weapon });
    }
}

public class WeaponReloadArgs:EventArgs
{
    public Weapon weapon;
}